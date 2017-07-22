using System.Collections.Generic;
using System.Windows;
using ClassLibrary.Models;

namespace OneClickUI.Views
{
    /// <summary>
    /// Логика взаимодействия для List.xaml
    /// </summary>
    public partial class AnyListView : Window
    { 
        public AnyListView(IList<BaseEntityModel> newlist)
        {
            InitializeComponent();
            this.Anylist.ItemsSource = newlist;
        }
    }
}
