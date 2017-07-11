using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ClassLibrary.Models;

namespace OneClickUI.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для dialog_categories.xaml
    /// </summary>
    public partial class dialog_categories : Window
    {

        public ObservableCollection<CategoryModel> categories ;
        private CategoryModel newItem;
        private BaseEntityModel newKey;

        public dialog_categories()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        public dialog_categories(ObservableCollection<CategoryModel> setCategories)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.categories = new ObservableCollection<CategoryModel>(setCategories);
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

        }



//----- Интерфейс
        private void btn_devRemove_Click(object sender, RoutedEventArgs e)
        {
            CategoryModel item = lst_Devices.SelectedItem as CategoryModel;
            if (item != null)
            {
                categories.Remove(item);
            }
        }

        private void btn_devAdd_Click(object sender, RoutedEventArgs e)
        {
            newItem = new CategoryModel();

            dialog_newItem wnd_newItem = new dialog_newItem(newItem);
            wnd_newItem.Show();
            wnd_newItem.btn_OK.Click += new RoutedEventHandler(wnd_newItem_btn_OK_Click);
        }

        private void btn_keyAdd_Click(object sender, RoutedEventArgs e)
        {
            newKey = new BaseEntityModel();

            dialog_newKey wnd_newKey = new dialog_newKey(newKey);
            wnd_newKey.Show();
            wnd_newKey.btn_OK.Click += new RoutedEventHandler(btn_OK_Click);
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            CategoryModel item = lst_Devices.SelectedItem as CategoryModel;
            if (item != null)
            {
                if (!newKey.Name.Equals(""))
                    item.Keys.Add(new KeyModel { Name = newKey.Name });
            }
        }

        private void btn_keyRemove_Click(object sender, RoutedEventArgs e)
        {
            CategoryModel item = lst_Devices.SelectedItem as CategoryModel;
            if (item != null)
            {
                item.Keys.Remove(new KeyModel { Name = item.SelectedKey });
            }
        }


        private void lst_Keys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = sender as ListBox;
            CategoryModel item = lst_Devices.SelectedItem as CategoryModel;
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
