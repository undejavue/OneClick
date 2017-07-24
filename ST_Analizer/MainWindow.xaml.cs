using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using OneClickUI.ViewModels;
using AboutView = OneClickUI.Views.AboutView;

namespace OneClickUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainViewModel viewModel;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel = new MainViewModel();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            foreach (var item in viewModel.LogFIlter)
            {
                Button btn = new Button {Content = item.Name,  Command = viewModel.FilterLogCommand, FontSize = 12};
                btn.CommandParameter = btn.Content;
                btn.Click += (sender, args) => { item.IsSelected = !item.IsSelected; };
                PanelLogFilter.Children.Add(btn);
            }
        }

     
        //--- Создать базу данных
        private void BtnDBcreateClick(object sender, RoutedEventArgs e)
        {
            
            //if (!bgWorker.IsBusy)
            //{
            //    BaseEntityModel arg = new BaseEntityModel();
            //    arg.Id = 4;
            //    arg.Name = "CreateDataBase";
            //    arg.Description = "Создание базы данных из коллекции объетов в памяти";
            //    bgWorker.RunWorkerAsync(arg);
            //}
            //else
            //    MessageBox.Show("Уже идет выполнение фоновой операции");
            //LabelProcess.Content = "Выполняется...";
            //OneClickDbCreate();
            //LabelProcess.Content = "Готово";
        }

        //--- Открыть базу данных
        private void BtnDBopenClick(object sender, RoutedEventArgs e)
        {
            OneClickDbOpen();
        }


     
        /// <summary>
        /// Создание новой базы данных из коллекции объектов в памяти
        /// </summary>
        private void OneClickDbCreate()
        {
            //bgWorker.ReportProgress(10, "Начата генерация базы данных");

            //if (categories != null)
            //{
            //    var frameView = new MainFramesView(categories);

            //    //bgWorker.ReportProgress(90, "База данных создана");
            //    frameView.Show();
            //}
            //else bgWorker.ReportProgress(100, "Не из чего создавать");

            //e.Result = new BaseEntityModel(4, "Выполнено!", "Генерация базы данных завершена");
        }


        /// <summary>
        /// Открыть существующую базу данных сигналов 
        /// из файла на диске
        /// </summary>
        private void OneClickDbOpen()
        {
            var dir = "RootDir";//G.rootdir;
            var filename = "";

            var openDlg = new OpenFileDialog();

            openDlg.InitialDirectory = dir;
            openDlg.Filter = "Database files (*.mdf)|*.mdf;|All Files (*.*)|*.*";

            // Set filter for file extension and default file extension
            openDlg.DefaultExt = ".mdf";

            // Display OpenFileDialog by calling ShowDialog method
            var result = openDlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                var safename = openDlg.SafeFileName;
                filename = openDlg.FileName;

                dir = filename.Remove(filename.Length - safename.Length - 1);


                //G.DBfilename = filename;

                var frameView = new Views.MainFramesView(filename);
                frameView.Show();
            }

        }

        //--- Работа главного меню ----------------------------------------
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutView();
            about.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_OpenDB_Click(object sender, RoutedEventArgs e)
        {
            OneClickDbOpen();

        }

        private void MenuItem_CreateDB_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

        }
    }
}
