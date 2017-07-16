using System.Windows;
using ClassLibrary.Models;

namespace OneClickUI.Views
{
    /// <summary>
    /// Interaction logic for NewCategoryKeyView.xaml
    /// </summary>
    public partial class NewCategoryKeyView : Window
    {
        public NewCategoryKeyView(KeyModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
