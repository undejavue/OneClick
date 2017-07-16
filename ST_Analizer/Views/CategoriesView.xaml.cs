using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ClassLibrary.Models;
using OneClickUI.ViewModels;
using OneClickUI.Helpers;

namespace OneClickUI.Views
{
    /// <summary>
    /// Логика взаимодействия для CategoriesView.xaml
    /// </summary>
    public partial class CategoriesView : Window
    {
        public CategoriesViewModel viewModel { get; set; }
        //public ObservableCollection<CategoryModel> categories ;
        //private CategoryModel newItem;
        //private BaseEntityModel newKey;

        public CategoriesView(ObservableRangeCollection<CategoryModel> categories)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = viewModel = new CategoriesViewModel(categories);
        }


//----- Интерфейс
        //private void btn_devRemove_Click(object sender, RoutedEventArgs e)
        //{
        //    viewModel.
        //}

        //private void btn_devAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    newItem = new CategoryModel();

        //    dialog_newItem wnd_newItem = new dialog_newItem(newItem);
        //    wnd_newItem.Show();
        //    wnd_newItem.btn_OK.Click += new RoutedEventHandler(wnd_newItem_btn_OK_Click);
        //}

        //private void btn_keyAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    newKey = new BaseEntityModel();

        //    dialog_newKey wnd_newKey = new dialog_newKey(newKey);
        //    wnd_newKey.Show();
        //    wnd_newKey.btn_OK.Click += new RoutedEventHandler(btn_OK_Click);
        //}

        //private void btn_OK_Click(object sender, RoutedEventArgs e)
        //{
        //    CategoryModel item = LstDevices.SelectedItem as CategoryModel;
        //    if (item != null)
        //    {
        //        if (!newKey.Name.Equals(""))
        //            item.Keys.Add(new KeyModel { Name = newKey.Name });
        //    }
        //}

        //private void btn_keyRemove_Click(object sender, RoutedEventArgs e)
        //{
        //    CategoryModel item = LstDevices.SelectedItem as CategoryModel;
        //    if (item != null)
        //    {
        //        item.Keys.Remove(new KeyModel { Name = item.SelectedKey });
        //    }
        //}


        //private void lst_Keys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ListBox list = sender as ListBox;
        //    CategoryModel item = LstDevices.SelectedItem as CategoryModel;
        //    if (item != null)
        //    {
        //        string k = item.SelectedKey;
        //    }
        //}

        //private void wnd_newItem_btn_OK_Click(object sender, RoutedEventArgs e)
        //{
        //    if (newItem != null)
        //        categories.Add(newItem);
        //}

        //private void btn_SetDefaults_Click(object sender, RoutedEventArgs e)
        //{
        //    OneClick_SetDefaultCategories();
        //}

        //private void btn_SaveChanges_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Close();
        //}

    }
}
