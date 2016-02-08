using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

using System.Windows;

using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

using ClassLibrary;

namespace OneClick_Analyser
{
    class ExcelWorks
    {
        private Excel.Application excelapp;
        private Excel.Workbook excelbook;

        //public string tableSheetName = "SymbolTable";
        private string _rootdir;
        public string rootdir
        {
            get { return _rootdir; }
            set { _rootdir = value; }
        }

        private bool _showExcel;
        public bool showExcel
        {
            get { return this._showExcel; }
            set { this._showExcel = value; }
        }

        private string _filename;
        public string fileName
        {
            get { return this._filename; }
            set { this._filename = value; }
        }

        public delegate void OneClickEventHandler(
        object sender,
        OneClickEventArgs args);

        public event OneClickEventHandler ReportMessage;

        protected virtual void OnReportMessage(string message)
        {
            if (ReportMessage != null)
            {
                ReportMessage(this, new OneClickEventArgs(message));
            }
        }


        public ExcelWorks(string dir) 
        {
            if (string.IsNullOrEmpty(dir))
                rootdir = System.Environment.CurrentDirectory;
            else
                rootdir = dir;
        }

        /// <summary>
        /// Интерактивный диалог выбора файла
        /// </summary>
        /// <returns>Имя рабочей директории</returns>
        public string OpenExcel()
        {
            string dir = rootdir;

            OpenFileDialog openDlg = new OpenFileDialog();

            openDlg.InitialDirectory = dir;
            openDlg.Filter = "Excel documents (*.xls, *.xlsm)|*.xls; *.xlsx; *.xslm|All Files (*.*)|*.*";

            // Set filter for file extension and default file extension
            openDlg.DefaultExt = ".xls";

            // Display OpenFileDialog by calling ShowDialog method
            bool? result = openDlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                string safename = openDlg.SafeFileName;
                string filename = openDlg.FileName;

                dir = filename.Remove(filename.Length - safename.Length - 1);

                // Create backup of xlsfile
                try
                {
                    File.Copy(openDlg.FileName, dir + "\\backCopy_" + safename, true);
                    OnReportMessage("Резервная копия файла конфигурации сохраннена");
                }
                catch
                {
                    OnReportMessage("Не удалось сделать резервную копию файла конфигурации");
                }

                // Open document
                excelapp = new Excel.Application();
                excelapp.Visible = true;
                
                excelbook = excelapp.Workbooks.Open(filename);

                OnReportMessage("Открыт файл конфигурации " + filename);

                
                
                fileName = filename;
            }

            OnReportMessage("Задана рабочая директория " + dir);

            rootdir = dir;
            return dir;
        }

        public bool setVisible(bool visible)
        {
            if ( (this.excelapp != null) & (this.excelbook != null) )
            {
                excelapp.Visible = visible;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Закрыть файл конфигурации
        /// </summary>
        /// <param name="saveChanges">Сохранить изменения</param>
        public void closeExcel(bool saveChanges)
        {
            try
            {
                if (excelapp != null)
                {
                    if (excelbook != null)
                    {
                        if (saveChanges)
                        {
                            excelbook.SaveAs(rootdir + "\\JobDone.xlsx");
                            excelbook.Close(Excel.XlSaveAction.xlSaveChanges);
                            OnReportMessage("Файл конфигурации сохранен под именем 'JobDone.xlsx'");
                        }
                        else
                        {
                            excelbook.Close(Excel.XlSaveAction.xlDoNotSaveChanges);
                            OnReportMessage("Файл конфигурации закрыт без сохранения");
                        }
                        excelbook = null;

                        //Marshal.ReleaseComObject(excelappworkbook);
                    }

                    excelapp.Quit();
                    Marshal.ReleaseComObject(excelapp);
                    excelapp = null;

                }
            }
            catch (Exception e)
            {
                OnReportMessage("Неудачные манипуляции с файлом Excel, не могу закрыть, так как закрыли вручную...");
                OnReportMessage("Исключение: " + e.Message.ToString());
            }
        }

        /// <summary>
        /// Создание строкового двумерного массива из листа Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public string[,] generate_ArrayFromRange(string sheetName, bool withClear)
        {
            
            var arr = new string[1, 1];

            if (excelbook != null)
            {
                Excel.Worksheet destSheet = excelbook.Worksheets.get_Item(sheetName);
                int last = destSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;

                //--- be careful, size of array must be like symbol table, 11xXX
                Excel.Range xRng = destSheet.Range["A1:K" + last.ToString()];
                arr = new string[xRng.Rows.Count, xRng.Columns.Count];

                foreach (Excel.Range item in xRng)
                {
                    arr[item.Row - 1, item.Column - 1] = Convert.ToString(item.Value);
                }

                if (withClear) xRng.Clear();
            }

            OnReportMessage("Сгенерирована таблица из листа " + sheetName);
            return arr;
        }

        private List<string> generateListFromRange(Excel.Range inputRng)
        {
            object[,] cellValues = (object[,])inputRng.Value2;
            List<string> lst = cellValues.Cast<object>().ToList().ConvertAll(x => Convert.ToString(x));
            return lst;
        }


        public void printArrayToSheet(String[,] arr, string sheetName)
        {
            if ( (excelbook != null) & (arr.GetLength(0) > 0) )
            {
                Excel.Worksheet destSheet = null;

                //----- Добавление листа
                if (!(isSheetExist(sheetName)))
                {
                    destSheet = excelbook.Worksheets.Add();
                    destSheet.Name = sheetName;
                }
                else destSheet = excelbook.Worksheets.get_Item(sheetName);

                destSheet.Select();
                destSheet.UsedRange.Clear();

                Excel.Range rng = destSheet.get_Range("A1", System.Reflection.Missing.Value).get_Resize(arr.GetLength(0), arr.GetLength(1));
                rng.set_Value(System.Reflection.Missing.Value, arr);

                OnReportMessage("Выгружены данные в лист " + sheetName);
            }
        }


        public void printArrayToSheetTemplate(String[,] arr, string sheetName)
        {
            if ( (excelbook != null) & (arr.GetLength(0) > 0) )
            {
                Excel.Worksheet destSheet = null;

                //----- Добавление листа на базе шаблона
                if (!(isSheetExist(sheetName)))
                {
                    destSheet = excelbook.Worksheets.get_Item("template");
                    destSheet.Copy(excelbook.Worksheets.get_Item("template"));
                    destSheet = excelbook.Worksheets.get_Item("template (2)");
                    destSheet.Name = sheetName;
                }
                else destSheet = excelbook.Worksheets.get_Item(sheetName);
                destSheet.Select();
                destSheet.UsedRange.Clear();

                Excel.Range rng = destSheet.get_Range("A1", System.Reflection.Missing.Value).get_Resize(arr.GetLength(0), arr.GetLength(1));
                rng.set_Value(System.Reflection.Missing.Value, arr);

                OnReportMessage("Выгружены данные в лист " + sheetName);
            }
        }



        /// <summary>
        /// Удаление списка листов из книги
        /// </summary>
        /// <param name="listNames">Список имен листов</param>
        public void deleteLists(List<string> listNames)
        {
            if (this.excelbook != null)
            {
                foreach (string listName in listNames)
                {
                    foreach (Excel.Worksheet sh in excelbook.Worksheets)
                    {
                        if (sh.Name.Equals(listName))
                        {
                            sh.Delete();
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Проверка существования листа в книге
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool isSheetExist(string name)
        {
            foreach (Excel.Worksheet sh in excelbook.Worksheets)
            {
                if (sh.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }


        public void deleteEmptyRows(Excel.Worksheet sh)
        {

            Excel.Range currentRow = null;
            Excel.Range rows = sh.Rows;
            int currentIndex = sh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            bool contentFound = false;
            while (!contentFound && currentIndex > 0)
            {
                currentRow = rows[currentIndex];
                if (excelapp.WorksheetFunction.CountA(currentRow) == 0) currentRow.Delete();
                //else contentFound = true;

                Marshal.FinalReleaseComObject(currentRow);
                currentIndex--;
            }

        }


        /// <summary>
        /// Резервное копирование листа
        /// </summary>
        /// <param name="sheetName"></param>
        public void excel_backupSheet(string sheetName)
        {
            foreach (Excel.Worksheet sh in excelbook.Worksheets)
            {
                if (sh.Name.Equals(sheetName))
                {
                    sh.Copy(Type.Missing, excelbook.Worksheets[excelbook.Worksheets.Count]);
                }
            }
        }


        /// <summary>
        /// Поиск на листе Excel по частичным текстовым совпадениям строк и ключей
        /// </summary>
        /// <param name="sourceSheet">Лист для поиска, источник</param>
        /// <param name="key">Ключ для текстового поиска</param>
        /// <returns>Список строк, преобразованный в список элементов класса символьной таблицы</returns>
        private List<mSymbolTableItem> OneClick_SearchKey(string sourceSheet, string key)
        {
            Excel.Sheets sheets = excelbook.Worksheets;
            Excel.Worksheet sheet = (Excel.Worksheet)sheets.get_Item(sourceSheet);
            sheet.Select();

            Excel.Range currentFind = null;
            Excel.Range firstFind = null;
            Excel.Range oneRow = null;

            List<mSymbolTableItem> s7_list = new List<mSymbolTableItem>();

            //------ Поиск ------------------------------------------
            currentFind = excelapp.Columns.Find(key, Type.Missing,
                                                Excel.XlFindLookIn.xlValues,
                                                Excel.XlLookAt.xlPart,
                                                Excel.XlSearchOrder.xlByRows,
                                                Excel.XlSearchDirection.xlNext, false);

            while (currentFind != null) //&(final<10) )
            {

                // Keep track of the first range you find. 
                if (firstFind == null)
                {
                    firstFind = currentFind;
                }

                // If you didn't move to a new range, you are done.
                else if (currentFind.get_Address(Type.Missing, Type.Missing, Excel.XlReferenceStyle.xlA1, Type.Missing, Type.Missing)
                      == firstFind.get_Address(Type.Missing, Type.Missing, Excel.XlReferenceStyle.xlA1, Type.Missing, Type.Missing))
                {
                    break;
                }

                int r = currentFind.Row;
                mSymbolTableItem s7_item = new mSymbolTableItem();

                // Строка символьной таблицы, [А234:K234]
                oneRow = sheet.Range["A" + r.ToString() + ":K" + r.ToString()];
                //s7_item = generateListFromRange(oneRow);

                s7_list.Add(s7_item);

                currentFind.EntireRow.Clear();
                currentFind = excelapp.Columns.FindNext(currentFind);
            }
            return s7_list;
        }    


        private void someStuffCode()
        {
            // -- Сортировка таблицы----------------------------
            //Rng = destSheet.Range["A:K"];
            //Rng.Sort(
            //        Rng.Columns[7], Excel.XlSortOrder.xlAscending,
            //        Rng.Columns[9], Type.Missing, Excel.XlSortOrder.xlAscending,
            //        Type.Missing, Excel.XlSortOrder.xlAscending,
            //        Excel.XlYesNoGuess.xlNo, Type.Missing, Type.Missing,
            //        Excel.XlSortOrientation.xlSortColumns,
            //        Excel.XlSortMethod.xlPinYin,
            //        Excel.XlSortDataOption.xlSortNormal,
            //        Excel.XlSortDataOption.xlSortNormal,
            //        Excel.XlSortDataOption.xlSortNormal);

            //Rng = destSheet.Range["A1:A" + last.ToString()];
            //symbols = rangeToList(Rng);

            // --- Find last row in Excel
            //int r = destSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
        }
    }

    class OneClickEventArgs : EventArgs
    {
        private readonly string _message;
        public string message
        {
            get { return this._message; }
        }

        public OneClickEventArgs(string text)
        {
            this._message = text;
        }


    }
}
