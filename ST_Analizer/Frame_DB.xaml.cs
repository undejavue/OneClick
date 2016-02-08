using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.Linq;
using System.Collections.ObjectModel;
//using Microsoft.Win32;
using System.IO;

using ClassLibrary;

namespace OneClick_Analyser
{
    /// <summary>
    /// Логика взаимодействия для DB_SymbolsEx.xaml
    /// </summary>
    public partial class Frame_DB : Page
    {

        public ObservableCollection<mCategory> collection;
        public string rootdir;

        public Frame_DB(ObservableCollection<mCategory> datasource)
        {
            InitializeComponent();
            //DataContext = this;
            collection = new ObservableCollection<mCategory>(datasource);
            Loaded += Frame_DB_Loaded;

        }

        private void Frame_DB_Loaded(object sender, RoutedEventArgs e)
        {
            dg_Categories.ItemsSource = collection;
            dg_Categories.SelectionChanged += dg_Categories_SelectionChanged;
            dg_Items.SelectionChanged += dg_Items_SelectionChanged;

        }

        public bool flush()
        {
            if (this.collection != null)
            {
                try
                {
                    collection.Clear();
                    return true;
                }

                catch(Exception ex)
                {
                    return false;
                }         
            }
            return false;
        }

        public void setCollection(ObservableCollection<mCategory> newCollection)
        {
            this.collection = new ObservableCollection<mCategory>(newCollection);
        }
      

        private void dg_Items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mSymbolTableItem item = ((DataGrid)sender).SelectedItem as mSymbolTableItem;

            if (item != null)
            {
                //dg_peripheryCode.ItemsSource = item.peripheryCode;
                list_peripheryCode.ItemsSource = item.peripheryCode;
            }

        }

        private void dg_Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                mCategory mc = ((DataGrid)sender).SelectedItem as mCategory;

                if (mc != null)
                {

                    grid_DataBlock.DataContext = mc.DB;
                    dg_Keys.ItemsSource = mc.Keys;
                    dg_Items.ItemsSource = mc.S7items;
                }
            }

        }

        private void btn_export2txt_Click(object sender, RoutedEventArgs e)
        {
            mCategory mc = dg_Categories.SelectedItem as mCategory;
            List<mCategory> cl = new List<mCategory>();
            cl.Add(mc);

            SourceGenerator SC = new SourceGenerator(cl);
            
            List<string> slist = SC.print_PeripheryForCategory(mc.Name);

            dialog_outBuffer outWindow = new dialog_outBuffer(slist);
            outWindow.Show();
        }

        private void btn_exportAll_Click(object sender, RoutedEventArgs e)
        {
            SourceGenerator SC = new SourceGenerator(collection.ToList());
            SC.printAllSourcesToFiles(rootdir);

            
            

        }

    }
}
