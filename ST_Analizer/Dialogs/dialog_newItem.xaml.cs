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
using System.Windows.Shapes;
using ClassLibrary;

namespace OneClick_Analyser
{
    /// <summary>
    /// Логика взаимодействия для dialog_newItem.xaml
    /// </summary>
    public partial class dialog_newItem : Window
    {

        mCategory item;

        public dialog_newItem(mCategory rItem)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Loaded += new RoutedEventHandler(dialog_newItem_Loaded);
            this.item = rItem;

        }

        void dialog_newItem_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.DataContext = item;

        }

        public void btn_OK_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        public void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.item = null;
            this.Close();
        }
    }
}
