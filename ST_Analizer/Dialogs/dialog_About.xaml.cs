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


namespace OneClick_Analyser
{
    /// <summary>
    /// Логика взаимодействия для dialog_About.xaml
    /// </summary>
    public partial class dialog_About : Window
    {
        public dialog_About()
        {
            InitializeComponent();
            this.txt_version.Text = setVersion();

        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private string setVersion(){


            Version version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

            return version.ToString();
        }

    }
}
