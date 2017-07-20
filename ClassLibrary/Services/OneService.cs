using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ClassLibrary.Excel;
using ClassLibrary.Models;

namespace ClassLibrary.Services
{
    public class OneService
    {

        public static DataTable GenerateTableFromExcel(string pathToExcel)
        {
            //var path = @"s:\OneClickDb\PLC1.xlsx";

            var dt = new DataTable();
            using (var reader = new ExcelDataReader(pathToExcel))
            {
                dt.Load(reader);
                return dt;
            } 
        }

        public static async Task<IEnumerable<CategoryModel>> GenerateDefaultCategoriesAsync()
        {
            var categories = new List<CategoryModel>();

            await Task.Run(() =>
            {
                CategoryModel cat = new CategoryModel(1, "A", "Датчики 4-20");
                cat.Db.UdtNumber = "1";
                cat.Db.UdtName = "SNS_UDT";
                cat.Db.Symbol = "A";
                cat.Db.ArrayName = "SNS";
                cat.FCname = "periphery_SNS";
                cat.Keys.Add(new KeyModel { Name = "IW" });
                categories.Add(cat);

                cat = new CategoryModel(3, "L", "Датчики уровня");
                cat.Db.UdtNumber = "3";
                cat.Db.UdtName = "SNL_UDT";
                cat.Db.Symbol = "D";
                cat.Db.ArrayName = "SNL";
                cat.FCname = "periphery_SNL";
                cat.Keys.Add(new KeyModel { Name = "уровень" });
                categories.Add(cat);

                cat = new CategoryModel(9, "C", "Счетчики");
                cat.Db.UdtNumber = "9";
                cat.Db.UdtName = "SNC_UDT";
                cat.Db.ArrayName = "SNC";
                cat.Db.Symbol = "SNC";
                cat.FCname = "periphery_SNC";
                cat.Keys.Add(new KeyModel { Name = "Импульсный выход" });
                cat.Keys.Add(new KeyModel { Name = "Счетчик" });
                categories.Add(cat);

                cat = new CategoryModel(10000, "PID", "ПИД-регуляторы");
                cat.Db.UdtNumber = "1";
                cat.Db.UdtName = "PID";
                cat.Db.ArrayName = "PID";
                cat.Db.Symbol = "PID";
                cat.FCname = "periphery_PID";
                cat.Keys.Add(new KeyModel { Name = "Регулятор" });
                cat.Keys.Add(new KeyModel { Name = "Позиционер" });
                categories.Add(cat);

                cat = new CategoryModel(2, "B", "Дискретные сигналы");
                cat.Db.UdtNumber = "2";
                cat.Db.Symbol = "B";
                cat.Db.ArrayName = "SNB";
                cat.Db.UdtName = "SNB_UDT";
                cat.FCname = "periphery_SNB";
                cat.Keys.Add(new KeyModel { Name = "атчик" });
                cat.Keys.Add(new KeyModel { Name = "фланш-панел" });
                cat.Keys.Add(new KeyModel { Name = "Датчик ФП" });
                cat.Keys.Add(new KeyModel { Name = "калача" });
                cat.Keys.Add(new KeyModel { Name = "Соединение" });
                categories.Add(cat);

                cat = new CategoryModel(4, "Y", "Клапаны");
                cat.Db.UdtNumber = "4";
                cat.Db.Symbol = "Y";
                cat.Db.ArrayName = "DRV";
                cat.FCname = "periphery_VLV";
                cat.Db.UdtName = "DRV_UDT";
                cat.Keys.Add(new KeyModel { Name = "клапан" });
                cat.Keys.Add(new KeyModel { Name = "мембр" });
                cat.Keys.Add(new KeyModel { Name = "невмоцилиндр" });
                //cat.Keys.Add("озатор");
                categories.Add(cat);

                cat = new CategoryModel(7, "MIX", "Мешалки СИ");
                cat.Db.UdtNumber = "7";
                cat.Db.UdtName = "MIXER_CM_UDT";
                cat.Db.Symbol = "MIX";
                cat.Db.ArrayName = "MIX";
                cat.FCname = "periphery_MIXER_CM";
                cat.Keys.Add(new KeyModel { Name = "мешалка СИ" });
                cat.Keys.Add(new KeyModel { Name = "сыроизготовит" });
                cat.Keys.Add(new KeyModel { Name = "тормоз" });
                //cat.Keys.Add("СИ");
                categories.Add(cat);

                cat = new CategoryModel(99, "SC", "Задание скорости");
                cat.Db.UdtNumber = "1";
                cat.Db.Symbol = "SC";
                cat.Db.ArrayName = "SC";
                cat.Db.UdtName = "SC";
                cat.FCname = "periphery_SC";
                cat.Keys.Add(new KeyModel { Name = "Задание скорости" });
                categories.Add(cat);

                cat = new CategoryModel(5, "M", "Насосы");
                cat.Db.UdtNumber = "5";
                cat.Db.Symbol = "M";
                cat.Db.ArrayName = "DRV";
                cat.FCname = "periphery_PMP";
                cat.Db.UdtName = "DRV_UDT";
                cat.Keys.Add(new KeyModel { Name = "асос" });
                cat.Keys.Add(new KeyModel { Name = "пускател" });
                cat.Keys.Add(new KeyModel { Name = "ПЧ" });
                cat.Keys.Add(new KeyModel { Name = "озатор" });
                cat.Keys.Add(new KeyModel { Name = "вибросито" });
                cat.Keys.Add(new KeyModel { Name = "мешалк" });
                categories.Add(cat);

                cat = new CategoryModel(6, "mxr", "Мешалки 2х скоростные");
                cat.Db.UdtNumber = "5";
                cat.Db.Symbol = "MXR";
                cat.Db.ArrayName = "DRV";
                cat.Db.UdtName = "DRV_UDT";
                cat.FCname = "periphery_MIXER_2S";
                cat.Keys.Add(new KeyModel { Name = "мешалк" });
                categories.Add(cat);
            });

            return categories;
        }
    }
}
