using System.Windows;
using ClassLibrary.Models;

namespace OneClickUI.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для dialog_newItem.xaml
    /// </summary>
    public partial class dialog_newItem : Window
    {

        CategoryModel item;

        public dialog_newItem(CategoryModel rItem)
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
