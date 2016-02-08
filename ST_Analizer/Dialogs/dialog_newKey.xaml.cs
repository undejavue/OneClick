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
    /// Логика взаимодействия для dialog_newKey.xaml
    /// </summary>
    public partial class dialog_newKey : Window
    {
        mBaseEntity item;


        public dialog_newKey(mBaseEntity newItem)
        {
            InitializeComponent();


            this.item = newItem;

            Loaded += new RoutedEventHandler(dialog_newKey_Loaded);
        }

        public dialog_newKey() { }


        void dialog_newKey_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = item;
        }


        public void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            //key = txt_Key.Text;

            this.Close();
        }

        public void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.item = null;
            this.Close();
        }
    }
}
