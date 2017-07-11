using System.Collections.Generic;
using System.ComponentModel;

namespace ClassLibrary.Models
{
    public class DataBlockModel: BaseEntityModel
    {
        //[ForeignKey("CategoryModel")]
        public int CategoryId { get; set; }
            
        /// <summary>
        /// Название блока данных
        /// </summary>
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        /// <summary>
        /// Полное имя блока данных: DB101
        /// </summary>
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(FullName)));
            }
        }

        /// <summary>
        /// Символьное имя блока данных, '10A'
        /// </summary>
        private string _symbolName;
        public string SymbolName
        {
            get { return _symbolName; }
            set
            {
                _symbolName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(FullName)));
            }
        }

        /// <summary>
        /// Номер блока данных = номер системы, '10'
        /// </summary>
        private string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Number)));
            }
        }

        /// <summary>
        /// Символ дата блока = тип устройства, 'A'
        /// </summary>
        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Symbol)));
            }
        }

        /// <summary>
        /// Имя массива 'SNS'
        /// </summary>
        private string _arrayName;
        public string ArrayName
        {
            get { return _arrayName; }
            set
            {
                _arrayName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ArrayName)));
            }
        }

        /// <summary>
        /// Имя пользовательского блока UDT, 'SNS_UDT'
        /// </summary>
        private string _udtName;
        public string UdtName
        {
            get { return _udtName; }
            set
            {
                _udtName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(UdtName)));
            }
        }

        /// <summary>
        /// Номер UDT, '1', используется для форм. полного имени DB
        /// </summary>
        private string _udtNumber;
        public string UdtNumber
        {
            get { return _udtNumber; }
            set
            {
                _udtNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(UdtNumber)));
            }
        }

        /// <summary>
        /// Для формирования списка блоков данных в это поле записывается максимальный индекс!
        /// S7 индекс в массиве,  SNS[i]
        /// </summary>
        private int _arrayIndex;
        public int ArrayIndex
        {
            get { return _arrayIndex; }
            set
            {
                _arrayIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ArrayIndex)));
            }
        }


        /// <summary>
        /// Link to model CategoryModel
        /// </summary>
        public virtual CategoryModel CategoryModel { get; set; }


        public List<string> return_DBinRowForPrint()
        {
            var row = new List<string>
            {
                this.SymbolName,
                this.FullName,
                this.UdtName.Equals("PID") ? "FB1" : this.FullName,
                this.Title,
                this.UdtName,
                this.ArrayIndex.ToString()
            };

            return row;
        }
    }
}
