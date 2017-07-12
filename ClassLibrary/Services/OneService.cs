using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Excel;
using ClassLibrary.Models;
using Newtonsoft.Json;
using DocumentFormat.OpenXml;

namespace ClassLibrary.Services
{
    public class OneService
    {

        public static DataTable GenerateTableFromExcel(string pathToExcel)
        {
            var path = @"s:\OneClickDb\PLC1.xlsx";

            var dt = new DataTable();
            using (var reader = new ExcelDataReader(path))
            {
                dt.Load(reader);
                return dt;
            }
                

            
        }
    }
}
