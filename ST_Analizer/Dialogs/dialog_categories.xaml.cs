using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using ClassLibrary;

namespace OneClick_Analyser
{
    /// <summary>
    /// Логика взаимодействия для dialog_categories.xaml
    /// </summary>
    public partial class dialog_categories : Window
    {

        public ObservableCollection<mCategory> categories ;
        private mCategory newItem;
        private mBaseEntity newKey;

        public dialog_categories()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        public dialog_categories(ObservableCollection<mCategory> setCategories)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.categories = new ObservableCollection<mCategory>(setCategories);
            lst_Devices.ItemsSource = categories;

        }



        /// <summary>
        /// Формирование "по-умолчанию" списка категорий и ключей 
        /// для последующей обработки символьной таблицы
        /// </summary>
        private void OneClick_SetDefaultCategories()
        {
            if (categories.Count > 0)
                categories.Clear();

            mCategory cat = new mCategory(1, "A", "Датчики 4-20");
            cat.DB.UDT_Number = "1";
            cat.DB.UDT_Name = "SNS_UDT";
            cat.DB.Symbol = "A";
            cat.DB.ArrayName = "SNS";
            cat.FCname = "periphery_SNS";
            cat.Keys.Add(new mKey { Name = "IW" });
            categories.Add(cat);

            cat = new mCategory(3, "L", "Датчики уровня");
            cat.DB.UDT_Number = "3";
            cat.DB.UDT_Name = "SNL_UDT";
            cat.DB.Symbol = "D";
            cat.DB.ArrayName = "SNL";
            cat.FCname = "periphery_SNL";
            cat.Keys.Add(new mKey { Name = "уровень" });
            categories.Add(cat);

            cat = new mCategory(9, "C", "Счетчики");
            cat.DB.UDT_Number = "9";
            cat.DB.UDT_Name = "SNC_UDT";
            cat.DB.ArrayName = "SNC";
            cat.DB.Symbol = "SNC";
            cat.FCname = "periphery_SNC";
            cat.Keys.Add(new mKey { Name = "Импульсный выход" });
            cat.Keys.Add(new mKey { Name = "Счетчик" });
            categories.Add(cat);

            cat = new mCategory(10000, "PID", "ПИД-регуляторы");
            cat.DB.UDT_Number = "1";
            cat.DB.UDT_Name = "PID";
            cat.DB.ArrayName = "PID";
            cat.DB.Symbol = "PID";
            cat.FCname = "periphery_PID";
            cat.Keys.Add(new mKey { Name = "Регулятор" });
            cat.Keys.Add(new mKey { Name = "Позиционер" });
            categories.Add(cat);


            cat = new mCategory(2, "B", "Дискретные сигналы");
            cat.DB.UDT_Number = "2";
            cat.DB.Symbol = "B";
            cat.DB.ArrayName = "SNB";
            cat.DB.UDT_Name = "SNB_UDT";
            cat.FCname = "periphery_SNB";
            cat.Keys.Add(new mKey { Name = "атчик" });
            cat.Keys.Add(new mKey { Name = "фланш-панел" });
            cat.Keys.Add(new mKey { Name = "Датчик ФП" });
            cat.Keys.Add(new mKey { Name = "калача" });
            cat.Keys.Add(new mKey { Name = "Соединение" });
            categories.Add(cat);

            cat = new mCategory(4, "Y", "Клапаны");
            cat.DB.UDT_Number = "4";
            cat.DB.Symbol = "Y";
            cat.DB.ArrayName = "DRV";
            cat.FCname = "periphery_VLV";
            cat.DB.UDT_Name = "DRV_UDT";
            cat.Keys.Add(new mKey { Name = "клапан" });
            cat.Keys.Add(new mKey { Name = "мембр" });
            cat.Keys.Add(new mKey { Name = "невмоцилиндр" });
            //cat.Keys.Add("озатор");
            categories.Add(cat);

            cat = new mCategory(7, "MIX", "Мешалки СИ");
            cat.DB.UDT_Number = "7";
            cat.DB.UDT_Name = "MIXER_CM_UDT";
            cat.DB.Symbol = "MIX";
            cat.DB.ArrayName = "MIX";
            cat.FCname = "periphery_MIXER_CM";
            cat.Keys.Add(new mKey { Name = "мешалка СИ" });
            cat.Keys.Add(new mKey { Name = "сыроизготовит" });
            cat.Keys.Add(new mKey { Name = "тормоз" });
            //cat.Keys.Add("СИ");
            categories.Add(cat);

            cat = new mCategory(99, "SC", "Задание скорости");
            cat.DB.UDT_Number = "1";
            cat.DB.Symbol = "SC";
            cat.DB.ArrayName = "SC";
            cat.DB.UDT_Name = "SC";
            cat.FCname = "periphery_SC";
            cat.Keys.Add(new mKey { Name = "Задание скорости" });
            categories.Add(cat);

            cat = new mCategory(5, "M", "Насосы");
            cat.DB.UDT_Number = "5";
            cat.DB.Symbol = "M";
            cat.DB.ArrayName = "DRV";
            cat.FCname = "periphery_PMP";
            cat.DB.UDT_Name = "DRV_UDT";
            cat.Keys.Add(new mKey { Name = "асос" });
            cat.Keys.Add(new mKey { Name = "пускател" });
            cat.Keys.Add(new mKey { Name = "ПЧ" });
            cat.Keys.Add(new mKey { Name = "озатор" });
            cat.Keys.Add(new mKey { Name = "вибросито" });
            cat.Keys.Add(new mKey { Name = "мешалк" });
            categories.Add(cat);

            cat = new mCategory(6, "mxr", "Мешалки 2х скоростные");
            cat.DB.UDT_Number = "5";
            cat.DB.Symbol = "MXR";
            cat.DB.ArrayName = "DRV";
            cat.DB.UDT_Name = "DRV_UDT";
            cat.FCname = "periphery_MIXER_2S";
            cat.Keys.Add(new mKey { Name = "мешалк" });
            categories.Add(cat);

        }



//----- Интерфейс
        private void btn_devRemove_Click(object sender, RoutedEventArgs e)
        {
            mCategory item = lst_Devices.SelectedItem as mCategory;
            if (item != null)
            {
                categories.Remove(item);
            }
        }

        private void btn_devAdd_Click(object sender, RoutedEventArgs e)
        {
            newItem = new mCategory();

            dialog_newItem wnd_newItem = new dialog_newItem(newItem);
            wnd_newItem.Show();
            wnd_newItem.btn_OK.Click += new RoutedEventHandler(wnd_newItem_btn_OK_Click);
        }

        private void btn_keyAdd_Click(object sender, RoutedEventArgs e)
        {
            newKey = new mBaseEntity();

            dialog_newKey wnd_newKey = new dialog_newKey(newKey);
            wnd_newKey.Show();
            wnd_newKey.btn_OK.Click += new RoutedEventHandler(btn_OK_Click);
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            mCategory item = lst_Devices.SelectedItem as mCategory;
            if (item != null)
            {
                if (!newKey.Name.Equals(""))
                    item.Keys.Add(new mKey { Name = newKey.Name });
            }
        }

        private void btn_keyRemove_Click(object sender, RoutedEventArgs e)
        {
            mCategory item = lst_Devices.SelectedItem as mCategory;
            if (item != null)
            {
                item.Keys.Remove(new mKey { Name = item.SelectedKey });
            }
        }


        private void lst_Keys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = sender as ListBox;
            mCategory item = lst_Devices.SelectedItem as mCategory;
            if (item != null)
            {
                string k = item.SelectedKey;
            }
        }

        private void wnd_newItem_btn_OK_Click(object sender, RoutedEventArgs e)
        {
            if (newItem != null)
                categories.Add(newItem);
        }

        private void btn_SetDefaults_Click(object sender, RoutedEventArgs e)
        {
            OneClick_SetDefaultCategories();
        }

        private void btn_SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
