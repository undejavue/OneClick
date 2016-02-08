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
    /// Логика взаимодействия для List.xaml
    /// </summary>
    public partial class anyList : Window
    {

        
        public anyList(List<mBaseEntity> newlist)
        {
            InitializeComponent();


            this.anylist.ItemsSource = newlist;
        }

    }
}
