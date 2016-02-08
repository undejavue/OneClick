using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;



namespace ClassLibrary
{
    /// <summary>
    /// Символьная таблица
    /// </summary>
    public class cSymbolTable: mBaseEntity
    {
        /// <summary>
        /// Коллекция объектов типа "Сигнал", она же - символьная таблица
        /// </summary>
        private ObservableCollection<mSymbolTableItem> _Symbols;
        public ObservableCollection<mSymbolTableItem> Symbols
        {
            get { return _Symbols; }
            set
            {
                _Symbols = value;
            }
        }

        public cSymbolTable ()
        {
            Symbols = new ObservableCollection<mSymbolTableItem>();
        }

        /// <summary>
        /// Создание символьной таблицы из одномерной коллекции строк (колонки таблицы)
        /// </summary>
        /// <param name="fromList">Колонка таблицы</param>
        public cSymbolTable(List<string> fromList)
        {
            Symbols = new ObservableCollection<mSymbolTableItem>();

            int i = 1;
            foreach (string line in fromList)
            {
                mSymbolTableItem item = new mSymbolTableItem();

                item.SignalName = line;
                Symbols.Add(item);

                i++;
            }
        }

        /// <summary>
        /// Создание символьной таблицы из массива, соответствующего таблице Excel
        /// </summary>
        /// <param name="arr">Символьная таблица в виде массива массива строк [4,xxx]</param>
        public cSymbolTable(String[,] arr)
        {
           Symbols = new ObservableCollection<mSymbolTableItem>();

           string s =  arr[1,1];

           for (int row = 0; row < arr.GetLength(0); row++)
            {                              
               mSymbolTableItem item = new mSymbolTableItem();

               item.SignalName = arr[row, 0];
               item.SignalAdress = arr[row, 1];
               item.SignalDataType = arr[row, 2];
               item.SignalComment = arr[row, 3];
               item.SignalType = arr[row, 4];
               item.Codename = arr[row,5];
               item.SystemNumber = arr[row, 6];
               item.DeviceType = arr[row, 7];
               item.DeviceNumber = arr[row, 8];               
               item.Etc = arr[row,9];
               item.DeviceTag = arr[row, 10];

               item.DB_FullName = item.SystemNumber;

               try
               {
                   item.DB_ArrayIndex = int.Parse(arr[row, 8]);
               }
               catch (Exception e)
               {
                   item.DB_ArrayIndex = 0;
               }


               Symbols.Add(item);
            }
        }

        /// <summary>
        /// Возвращает массив для выгрузки в Excel, состоящий из элементов коллекции Symbols
        /// </summary>
        /// <returns>array[,]</returns>
        public String[,] return_ArrayOfSymbols()
        {
            List<string> list = new List<string>();
            if (Symbols.Count > 0)
            {
                list = Symbols[0].return_ItemInOneRow();
            }

            var arr = new string[this.Symbols.Count, list.Count];
            int i = 0;

            foreach (mSymbolTableItem el in Symbols)
            {
                list = el.return_ItemInOneRow();
                int j = 0;
                foreach (string s in list)
                {
                    arr[i, j] = s;
                    j++;
                }
                i++;
            }

            return arr;
        }

        /// <summary>
        /// Очистка коллекции сигналов, т.е. удаление символьной таблицы из памяти
        /// </summary>
        /// <returns></returns>
        public bool clearSymbols()
        {
            this.Symbols.Clear();
            return true;
        }

        /// <summary>
        /// Возвращает истуну, если таблица символов пуста
        /// </summary>
        /// <returns></returns>
        public bool isEmpty()
        {
            bool isEmpty = true;

            if (this.Symbols.Count > 0) isEmpty = false;

            return isEmpty;
        }

        /// <summary>
        /// Анализ символьного имени, при соответствии критериям функция задает значение полю DeviceTag
        /// </summary>
        public void analyseAndSetTags()
        {
            foreach (mSymbolTableItem signal in this.Symbols)
            {

                string[] res = new string[3];
                string[] splited = signal.SignalName.Split(new char[] { '_' }, 3);  // Q_25M06SC_ON_RUN_ETC split: Q, 25M06SC, ON_RUN_ETC

                // splited[0] = Q
                // splited[1] = 25M06SC
                // splited[2] = ON_RUN_ETC

                if (splited.Count() > 1)
                {

                    signal.SignalType = splited[0];     // type 'I', 'Q', 'IW', 'QW' 
                    signal.Codename = splited[1];       // 25M06SC

                    if (splited[1].Length >= 4)
                    {
                        //-----------
                        string[] numbers = Regex.Split(splited[1], @"\D+");
                        if (!string.IsNullOrEmpty(numbers[0])) 
                        { 
                            signal.SystemNumber = numbers[0]; 
                        }
                        if (!string.IsNullOrEmpty(numbers[1])) 
                        { 
                            signal.DeviceNumber = numbers[1]; 
                        }

                        string[] NotNumbers = Regex.Split(splited[1], @"[0-9]");
                        foreach (string value in NotNumbers)
                            if (!string.IsNullOrEmpty(value))
                            {
                                signal.DeviceType = value;
                                break;
                            }

                        if (signal.DeviceType.Contains("U"))
                            signal.DeviceType = "M";
                    }
                }

                //--- ПРОДУМАТЬ БОЛЕЕЕ КРАСИВЫЙ СПОСОБ ВЫДЕЛИТЬ FIC из 25A100FIC
                if (splited.Count() > 2)
                {
                    signal.Etc = splited[2];
                }
                else
                {
                    int n;
                    bool isNumeric = false;

                    if (splited[1].Length > 5)
                    {
                        isNumeric = int.TryParse(splited[1].Substring(5, 1), out n);
                        if (!isNumeric)
                            signal.Etc = splited[1].Substring(5);
                    }

                }
                
                // Задать имя тегу для кода ПЛК, анализируя тип сигнала и текст ETC                                  
                signal.DeviceTag = setTag(signal.Etc, signal.SignalType);

                // Если не нашлось совпадений, то попробовать по типу устройства
                if (string.IsNullOrEmpty(signal.DeviceTag) & signal.DeviceType != null)
                {
                    signal.DeviceTag = setTagByDevType(signal.DeviceType);
                }

                // Если все еще не нашлось совпадений, то проанализировать комментарий
                if (string.IsNullOrEmpty(signal.DeviceTag))
                {
                    signal.DeviceTag = setTagByComment(signal.SignalComment);
                }

                try
                {
                    signal.DB_ArrayIndex = Convert.ToInt32(signal.DeviceNumber);
                }
                catch
                {
                    signal.DB_ArrayIndex = 0;
                }
                signal.DB_FullName = signal.SystemNumber;
            }
        }


        /// <summary>
        /// Значение тега в зависимости от совпадений в символьно имени
        /// </summary>
        /// <param name="etc">Часть символьного имени после всех значащих и уже обработанных частей</param>
        /// <param name="type">Тип сигнала в пространстве s7 symbol table</param>
        /// <returns>Возвращает имя тега для исходного кода в зависимости от совпадения части символьного имени</returns>
        private string setTag(string etc, string type)
        {
            string op = "";

            if ((etc != null) & (type.Equals("I")))
            {
                if (etc.Contains("ON") | etc.Contains("OFF") | etc.Contains("RUN") | etc.Contains("FS")) op = "I_on";
                if (etc.Contains("LL") | etc.Contains("LH") | etc.Contains("LM")) op = "Input";
                if (etc.Contains("COUNT") | etc.Equals("FC") | etc.Contains("FC") | etc.Contains("FI")) op = "Pulse";
                if (etc.Contains("QF")) op = "QF";
            }

            if (type != null) 
            {
                if (type == "Q")
                {
                    op = "Ctr";
                }

                if ((type == "PIW") | (type == "IW"))
                {
                    op = "Input";
                }

                if ((type == "PQW") | (type == "QW"))
                {
                    op = "Setpoint";
                }

            }

            return op;
        }

        /// <summary>
        /// Задать тег в зависимости от типа устройства
        /// </summary>
        /// <param name="comment">Тип устройства, Y, M, A, D, B и т.д.</param>
        /// <returns>Значение тега для исходного кода PLC</returns>
        private string setTagByDevType(string t)
        {
            string op = "";

            if (t.Equals("B")) op = "I_on";

            return op;
        }

        /// <summary>
        /// Задать тег в зависимости от содержимого комментария к сигналу
        /// </summary>
        /// <param name="comment">Комментарий к сигналу в таблице символов</param>
        /// <returns>Значение тега для исходного кода PLC</returns>
        private string setTagByComment(string comment)
        {
            string op = "";

            if (comment.Contains("уровень")) op = "Input";
            if (comment.Contains("Управление")) op = "Ctr";
            if (comment.Contains("Режим \"Ход\"") | comment.Contains("Датчик") | comment.Contains("Соединение") | comment.Contains("включен")) op = "I_on";
            if (comment.Contains("cост. пускателя")) op = "QF";
            if (comment.Contains("четчик") ) op = "Pulse";

            return op;
        }
        
        /// <summary>
        /// Сортировка по Номеру системы, затем по Типу устройства, затем по Номеру устройства
        /// </summary>
        public void sortTable()
        {
            this.Symbols = new ObservableCollection<mSymbolTableItem>(Symbols.OrderBy(key => key.SystemNumber).ThenBy(key => key.DeviceType).ThenBy(key => key.DeviceNumber));
        }


        public List<mSymbolTableItem> extractListByKeys(List<string> keys)
        {
            List<mSymbolTableItem> list = new List<mSymbolTableItem>();

            foreach (string key in keys)
            {
                foreach (mSymbolTableItem item in this.Symbols)
                {
                    if (isItemMatchKey(item, key))
                    {
                        list.Add(item);
                    }
                }

                foreach (mSymbolTableItem item in list)
                {
                    this.Symbols.Remove(item);
                }
            }
            return list;
        }

        private bool isItemMatchKey(mSymbolTableItem item, string key)
        {
            bool isMatch = false;

            if (item.SignalType.Equals(key)) return true;
            if (item.SignalComment.Contains(key)) return true;

            return isMatch;
        }

    }




}
