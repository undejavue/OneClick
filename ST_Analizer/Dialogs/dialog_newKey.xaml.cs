using System.Windows;
using ClassLibrary.Models;

namespace OneClickUI.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для dialog_newKey.xaml
    /// </summary>
    public partial class dialog_newKey : Window
    {
        BaseEntityModel item;


        public dialog_newKey(BaseEntityModel newItem)
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
