using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ClassLibrary.Models;

namespace OneClickUI.Views
{
    /// <summary>
    /// Interaction logic for NewCategoryView.xaml
    /// </summary>
    public partial class NewCategoryView : Window
    {

        public NewCategoryView(CategoryModel model)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = model;
        }

        public void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        public void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
