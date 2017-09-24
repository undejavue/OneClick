using System;
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
            try
            {
                using (var reader = new ExcelDataReader(pathToExcel))
                {
                    dt.Load(reader);
                    return dt;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        public static async Task<IEnumerable<CategoryModel>> GenerateDefaultCategoriesAsync()
        {
            var categories = new List<CategoryModel>();

            await Task.Run(() =>
            {
                CategoryModel cat = new CategoryModel(1, "A", "Sensors 4-20");
                cat.Db.UdtNumber = "1";
                cat.Db.UdtName = "SNS";
                cat.Db.Symbol = "A";
                cat.Db.ArrayName = "SNS";
                cat.FCname = "periphery_SNS";
                cat.Keys.Add(new KeyModel { Name = "IW" });
                categories.Add(cat);

                cat = new CategoryModel(3, "L", "Level sensors");
                cat.Db.UdtNumber = "3";
                cat.Db.UdtName = "SND";
                cat.Db.Symbol = "D";
                cat.Db.ArrayName = "SND";
                cat.FCname = "periphery_SNL";
                cat.Keys.Add(new KeyModel { Name = "уровень" });
                categories.Add(cat);

                cat = new CategoryModel(9, "C", "Counters");
                cat.Db.UdtNumber = "9";
                cat.Db.UdtName = "SNC";
                cat.Db.ArrayName = "SNC";
                cat.Db.Symbol = "SNC";
                cat.FCname = "periphery_SNC";
                cat.Keys.Add(new KeyModel { Name = "Импульсный выход" });
                cat.Keys.Add(new KeyModel { Name = "Счетчик" });
                categories.Add(cat);

                cat = new CategoryModel(10000, "PID", "PID control");
                cat.Db.UdtNumber = "1";
                cat.Db.UdtName = "PID";
                cat.Db.ArrayName = "PID";
                cat.Db.Symbol = "PID";
                cat.FCname = "periphery_PID";
                cat.Keys.Add(new KeyModel { Name = "Регулятор" });
                cat.Keys.Add(new KeyModel { Name = "Позиционер" });
                categories.Add(cat);

                cat = new CategoryModel(2, "B", "Discrette signals");
                cat.Db.UdtNumber = "2";
                cat.Db.Symbol = "B";
                cat.Db.ArrayName = "SNB";
                cat.Db.UdtName = "SND";
                cat.FCname = "periphery_SNB";
                cat.Keys.Add(new KeyModel { Name = "атчик" });
                cat.Keys.Add(new KeyModel { Name = "фланш-панел" });
                cat.Keys.Add(new KeyModel { Name = "Датчик ФП" });
                cat.Keys.Add(new KeyModel { Name = "калача" });
                cat.Keys.Add(new KeyModel { Name = "Соединение" });
                cat.Keys.Add(new KeyModel { Name = "Оптический датчик" });
                cat.Keys.Add(new KeyModel { Name = "Индикация" });
                cat.Keys.Add(new KeyModel { Name = "Пост" });
                categories.Add(cat);

                cat = new CategoryModel(4, "Y", "Valves");
                cat.Db.UdtNumber = "4";
                cat.Db.Symbol = "Y";
                cat.Db.ArrayName = "DRV";
                cat.FCname = "periphery_VLV";
                cat.Db.UdtName = "DRV";
                cat.Keys.Add(new KeyModel { Name = "клапан" });
                cat.Keys.Add(new KeyModel { Name = "мембр" });
                cat.Keys.Add(new KeyModel { Name = "невмоцилиндр" });
                //cat.Keys.Add("озатор");
                categories.Add(cat);

                cat = new CategoryModel(7, "MXR_CM", "Cheese Mixers");
                cat.Db.UdtNumber = "7";
                cat.Db.UdtName = "MXR_CM";
                cat.Db.Symbol = "MXR";
                cat.Db.ArrayName = "MXR";
                cat.FCname = "periphery_MXR_CM";
                cat.Keys.Add(new KeyModel { Name = "мешалка СИ" });
                cat.Keys.Add(new KeyModel { Name = "сыроизготовит" });
                cat.Keys.Add(new KeyModel { Name = "тормоз" });
                //cat.Keys.Add("СИ");
                categories.Add(cat);

                cat = new CategoryModel(99, "SC", "Setpoint");
                cat.Db.UdtNumber = "1";
                cat.Db.Symbol = "SC";
                cat.Db.ArrayName = "SC";
                cat.Db.UdtName = "SC";
                cat.FCname = "periphery_SC";
                cat.Keys.Add(new KeyModel { Name = "Задание скорости" });
                categories.Add(cat);

                cat = new CategoryModel(6, "MXR_2S", "Mixers 2-speed");
                cat.Db.UdtNumber = "5";
                cat.Db.Symbol = "MXR_2S";
                cat.Db.ArrayName = "DRV";
                cat.Db.UdtName = "MXR_2S";
                cat.FCname = "periphery_MXR_2S";
                cat.Keys.Add(new KeyModel { Name = "мешалк" });
                cat.Keys.Add(new KeyModel { Name = "скорость мешалк" });
                cat.Keys.Add(new KeyModel { Name = "UF11" });
                cat.Keys.Add(new KeyModel { Name = "UF12" });
                categories.Add(cat);


                cat = new CategoryModel(5, "M", "Pumps");
                cat.Db.UdtNumber = "5";
                cat.Db.Symbol = "M";
                cat.Db.ArrayName = "DRV";
                cat.FCname = "periphery_PMP";
                cat.Db.UdtName = "DRV";
                cat.Keys.Add(new KeyModel { Name = "асос" });
                cat.Keys.Add(new KeyModel { Name = "пускател" });
                cat.Keys.Add(new KeyModel { Name = "ПЧ" });
                cat.Keys.Add(new KeyModel { Name = "вкл. сборки" });
                cat.Keys.Add(new KeyModel { Name = "выкл. сборки" });
                cat.Keys.Add(new KeyModel { Name = "ПЧ" });
                cat.Keys.Add(new KeyModel { Name = "озатор" });
                cat.Keys.Add(new KeyModel { Name = "вибросито" });
                categories.Add(cat);
            });
            return categories;
        }
    }
}
