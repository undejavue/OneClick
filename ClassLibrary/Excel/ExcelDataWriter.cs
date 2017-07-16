using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json.Linq;


namespace ClassLibrary.Excel
{
    public class ExcelDataWriter
    {

        //public static void FastWrite(string outputPath, IEnumerable<SymbolTableItemModel> collection, string sheetName)
        //{
        //    using (var document = SpreadsheetDocument.Open(outputPath, true))
        //    {
        //        var workbook = document.WorkbookPart.Workbook;
        //        var workbookPart = document.WorkbookPart;

        //        var sheets = workbook.Sheets;
        //        var sheet = new Sheet { Name = sheetName};

        //        sheets.Append(sheet);
        //        workbookPart.Workbook.Save();
        //    }

        //    var outputFile = new FileInfo(outputPath);

        //    var q = collection.Select(x => new SymbolItemModel(x));

        //    using (var fastExcel = new FastExcel.FastExcel(outputFile))
        //    {
        //        fastExcel.Write(q, sheetName, false);
        //    }
        //}
        public static void BackupFile(string source)
        {
            var fi = new FileInfo(source);         
            var dest = Path.Combine(fi.DirectoryName, fi.Name + "_backup" + fi.Extension);
            File.Copy(source, dest);
        }

        public static void WriteExcelFromArray(string outputPath, string[,] array, string sheetName)
        {
            var fi = new FileInfo(outputPath);
            var dest = Path.Combine(fi.DirectoryName, fi.Name + "_result" + fi.Extension);

            using (var document = SpreadsheetDocument.Create(outputPath, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = sheetName };

                sheets.Append(sheet);
                int columnsCount = array.GetLength(0);
                int rowsCount = array.GetLength(1);

                for(int i = 0; i < array.GetLength(0); i++)
                {
                    var newRow = new Row();
                    for (int j = 0;j < array.GetLength(1); j++)
                    {
                        var cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(array[i,j]);
                        newRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(newRow);
                }
                workbookPart.Workbook.Save();
            }
        }

        public static void WriteExcelFromDatatable(string outputPath, DataTable table, string sheetName)
        {
            using (var document = SpreadsheetDocument.Create(outputPath, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = sheetName };

                sheets.Append(sheet);
               
                var headerRow = new Row();

                var columns = new List<string>();
                foreach (System.Data.DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);

                    var cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                foreach (DataRow dsrow in table.Rows)
                {
                    var newRow = new Row();
                    foreach (var col in columns)
                    {
                        var cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(dsrow[col].ToString());
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

                workbookPart.Workbook.Save();
            }
        }
    }
}
