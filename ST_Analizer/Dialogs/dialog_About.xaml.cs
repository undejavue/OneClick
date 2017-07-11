using System;
using System.Windows;

namespace OneClickUI.Dialogs
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
