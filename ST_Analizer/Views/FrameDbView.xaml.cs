using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using ClassLibrary.Models;
using ClassLibrary.SourceGenerator;

namespace OneClickUI.Views
{
    /// <summary>
    /// Логика взаимодействия для DB_SymbolsEx.xaml
    /// </summary>
    public partial class FrameDbView
    {

        public ObservableCollection<CategoryModel> Collection;
        public string Rootdir;

        public FrameDbView(ObservableCollection<CategoryModel> datasource)
        {
            InitializeComponent();
            Collection = new ObservableCollection<CategoryModel>(datasource);
            Loaded += FrameDbViewLoaded;
        }

        private void FrameDbViewLoaded(object sender, RoutedEventArgs e)
        {
            dg_Categories.ItemsSource = Collection;
            dg_Categories.SelectionChanged += DgCategoriesSelectionChanged;
            dg_Items.SelectionChanged += DgItemsSelectionChanged;

        }

        public bool Flush()
        {
            if (this.Collection != null)
            {
                try
                {
                    Collection.Clear();
                    return true;
                }

                catch(Exception ex)
                {
                    var m = ex.Message;
                    return false;
                }         
            }
            return false;
        }

        public void SetCollection(ObservableCollection<CategoryModel> newCollection)
        {
            this.Collection = new ObservableCollection<CategoryModel>(newCollection);
        }
      

        private void DgItemsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ((DataGrid)sender).SelectedItem as SymbolTableItemModel;

            if (item != null)
            {
                list_peripheryCode.ItemsSource = item.PeripheryCode;
            }

        }

        private void DgCategoriesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var mc = ((DataGrid) sender)?.SelectedItem as CategoryModel;
            if (mc == null) return;

            grid_DataBlock.DataContext = mc.Db;
            dg_Keys.ItemsSource = mc.Keys;
            dg_Items.ItemsSource = mc.S7Items;
        }

        private void BtnExport2TxtClick(object sender, RoutedEventArgs e)
        {
            var mc = dg_Categories.SelectedItem as CategoryModel;
            var cl = new List<CategoryModel>();
            cl.Add(mc);

            var sc = new SourceGenerator(cl);    
            var slist = sc.PrintPeripheryForCategory(mc.Name);

            var outWindow = new TextBufferView(slist);
            outWindow.Show();
        }

        private async void BtnExportAllClick(object sender, RoutedEventArgs e)
        {
            var sc = new SourceGenerator(Collection.ToList());
            await sc.PrintAllSourcesToFiles(Rootdir, new CancellationToken());
        }

    }
}
