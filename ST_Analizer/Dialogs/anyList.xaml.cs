using System.Collections.Generic;
using System.Windows;
using ClassLibrary.Models;

namespace OneClickUI.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для List.xaml
    /// </summary>
    public partial class anyList : Window
    {

        
        public anyList(List<BaseEntityModel> newlist)
        {
            InitializeComponent();


            this.anylist.ItemsSource = newlist;
        }

    }
}
