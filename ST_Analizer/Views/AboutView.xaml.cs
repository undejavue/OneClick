using System;
using System.Windows;

namespace OneClickUI.Views
{
    /// <summary>
    /// Логика взаимодействия для AboutView.xaml
    /// </summary>
    public partial class AboutView : Window
    {
        public AboutView()
        {
            InitializeComponent();
            this.TxtVersion.Content = SetVersion();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private static string SetVersion()
        {
            Version version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            return $"Версия {version}";
        }

    }
}
