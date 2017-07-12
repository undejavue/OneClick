using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ClassLibrary.Models;
using ClassLibrary.SourceGenerator;
using Microsoft.Win32;
using OneClickUI.Dialogs;
using OneClickUI.Excel;
using OneClickUI.ViewModels;

namespace OneClickUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainViewModel viewModel;


        private ObservableCollection<CategoryModel> categories;
        private SourceGenerator sources;
        private ExcelWorks exWorks;
        private dialog_categories wndCategories;
        private readonly BackgroundWorker bgWorker;

        // Globals properties class
        public Globals G;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            categories = new ObservableCollection<CategoryModel>();


            // Acync worker for continuous operation
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += BgWorkerDoWork;
            bgWorker.RunWorkerCompleted += BgWorkerRunWorkerCompleted;
            bgWorker.ProgressChanged += BgWorkerProgressChanged;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;

            //txt_filename.DataContext = global_FileName;
            OneClickSetGlobals();
        }

        //----- Асинхронное выполнение основных операций ------------------------------

        private void BgWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var arg = (BaseEntityModel)e.Argument;

            switch (arg.Id)
            {
                case 1:
                    OneClickTableAdaptation(e);
                    break;
                case 2:
                    OneClickTableToCollection(e);
                    break;
                case 3:
                    OneClickSourceGenerator(e);
                    break;
                case 4:
                    //OneClickDbCreate(e);
                    break;
                default:
                    break;
            }
        }

        private void BgWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.ProgressBar.Value = e.ProgressPercentage;
            this.TxtResult.AppendText("\r\n" + DateTime.Now.ToString("h:mm:ss") + ": " + e.UserState.ToString());
            this.TxtResult.ScrollToEnd();

            LabelProcess.Content = "Выполняется...";
        }

        private void BgWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Операция отменена", "S7 analyzer", MessageBoxButton.OK);
                LabelProcess.Content = "Отменено!";
                Print2Result("Операция отменена");
            }
            else
            {
                var result = (BaseEntityModel)e.Result;
                Print2Result(result.Description);
                LabelProcess.Content = result.Name;
            }

            ProgressBar.IsIndeterminate = false;
            this.ProgressBar.Value = 100;

            BtnCancel.IsEnabled = false;
            BtnGenSource.IsEnabled = true;

            exWorks.SetVisible(true);
        }

        //----- Интерфейс основных операций  -----------------------------------------


        //--- Конфигурация опций интерфейса
        private void OneClickSetGlobals()
        {
            G = new Globals();

            G.rootdir = "D:\\OneClickDB";

            GridFileOperations.DataContext = G;
            GridDbOperations.DataContext = G;

        }


        //--- Загрузка файла конфигурации
        private void BtnExcelOpenClick(object sender, RoutedEventArgs e)
        {
            if (exWorks == null)
            {
                exWorks = new ExcelWorks(G.rootdir);
                exWorks.ReportMessage += ExWorksReportMessage;
            }

            G.rootdir = exWorks.OpenExcel();

            BtnTblAdapt.IsEnabled = true;
            BtnOperations.IsEnabled = true;
            BtnGenSource.IsEnabled = true;
            BtnSave.IsEnabled = true;
            BtnClose.IsEnabled = true;

            G.filename = exWorks.fileName;
        }

        private void ExWorksReportMessage(object sender, OneClickEventArgs args)
        {
            if (bgWorker.IsBusy)
            {
                bgWorker.ReportProgress(50, args.Message);
            }
            else
            {
                this.TxtResult.AppendText("\r\n" + DateTime.Now.ToString("h:mm:ss") + ": " + args.Message);
            }
        }


        //--- Первоначальная обработка таблицы
        private void BtnTableAdaptationClick(object sender, RoutedEventArgs e)
        {
            if (!bgWorker.IsBusy)
            {
                var arg = new BaseEntityModel();
                arg.Id = 1;
                arg.Name = "Analyse";
                arg.Description = "Анализ таблицы сигналов";
                bgWorker.RunWorkerAsync(arg);

                BtnCancel.IsEnabled = true;
                exWorks.SetVisible(G.isExcelVisible);
            }
            else
                MessageBox.Show("Уже идет выполнение фоновой операции");
        }

        //--- Задание категорий для коллекции
        private void BtnSetCategoriesClick(object sender, RoutedEventArgs e)
        {
            wndCategories = new dialog_categories(categories);
            wndCategories.Show();
            G.categoriesCount = categories.Count;

            wndCategories.btn_SaveChanges.Click += wnd_Categories_SaveChanges_Click;
        }

        private void wnd_Categories_SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            categories = new ObservableCollection<CategoryModel>(wndCategories.categories);
            G.categoriesCount = categories.Count;
        }


        //--- Создание коллекции из таблицы и работа с ней
        private void BtnOperationsClick(object sender, RoutedEventArgs e)
        {
            if (!bgWorker.IsBusy)
            {
                var arg = new BaseEntityModel();
                arg.Id = 2;
                arg.Name = "Categorize";
                arg.Description = "Сортировка сигналов по таблицам категорий";
                bgWorker.RunWorkerAsync(arg);

                BtnCancel.IsEnabled = true;
                exWorks.SetVisible(G.isExcelVisible);
            }
            else
                MessageBox.Show("Уже идет выполнение фоновой операции");
        }

        //--- Генерация файлов исходного кода для PLC-программы
        private void BtnGenSourceClick(object sender, RoutedEventArgs e)
        {
            if (!bgWorker.IsBusy)
            {
                var arg = new BaseEntityModel();
                arg.Id = 3;
                arg.Name = "GenerateSources";
                arg.Description = "Генерация текстов исходных кодов";
                bgWorker.RunWorkerAsync(arg);
            }
            else
                MessageBox.Show("Уже идет выполнение фоновой операции");
        }

        //--- Отмена выполнения фоновых операций
        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            bgWorker.CancelAsync();
        }

        //--- Сохранить и закрыть файл конфигурации
        private void BtnSaveClick(object sender, RoutedEventArgs e)
        {
            exWorks.CloseExcel(true);
            G.filename = "";
        }

        //--- Закрыть файл конфигурации
        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            exWorks.CloseExcel(false);
            G.filename = "";
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
            LabelProcess.Content = "Выполняется...";
            OneClickDbCreate();
            LabelProcess.Content = "Готово";
        }

        //--- Открыть базу данных
        private void BtnDBopenClick(object sender, RoutedEventArgs e)
        {
            OneClickDbOpen();
        }


        //----- Основные функции --------------------------------------------------


        /// <summary>
        /// Анализ исходной таблицы символов, формирование дополнительных полей таблицы
        /// из символьного имени сигнала и комментария, сортировка таблицы
        /// </summary>
        /// <param name="e">Параметры асинхронного обработчика</param>
        private void OneClickTableAdaptation(DoWorkEventArgs e)
        {
            exWorks.Excel_backupSheet("SymbolTable");
            bgWorker.ReportProgress(5, "Резервная копия таблицы создана");

            if (bgWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            var symbolTableModel = new SymbolTableModel(exWorks.Generate_ArrayFromRange("SymbolTable", true));
            bgWorker.ReportProgress(30, "Таблица прочитана в память");

            if (bgWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            symbolTableModel.AnalyseAndSetTags();
            bgWorker.ReportProgress(60, "Анализ символьных имен выполнен");

            if (bgWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            symbolTableModel.SortTable();
            bgWorker.ReportProgress(70, "Сортировка выполнена");

            if (bgWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            var arr = symbolTableModel.GetSymbolsArray();
            exWorks.PrintArrayToSheet(arr, "SymbolTable");
            bgWorker.ReportProgress(98, "Таблица выгружена в Excel");

            symbolTableModel.ClearSymbols();

            e.Result = new BaseEntityModel(1, "Выполнено!", "Первичная обработка таблицы выполнена");
        }

        /// <summary>
        /// Формирование в памяти коллекции символьных списков,
        /// из исходной таблицы по заданным категориям и ключам
        /// </summary>
        /// <param name="e"></param>
        private void OneClickTableToCollection(DoWorkEventArgs e)
        {
            exWorks.Excel_backupSheet("SymbolTable");
            bgWorker.ReportProgress(10, "Запуск цикла сортировки таблицы по категориям...");

            if (categories.Count > 0)
            {
                var unsortedTableModel = new SymbolTableModel(exWorks.Generate_ArrayFromRange("SymbolTable", true));
                bgWorker.ReportProgress(40, "Таблица загружена в память");


                foreach (var cat in categories)
                {
                    if (bgWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    var list = new List<string>();
                    foreach (var k in cat.Keys)
                    {
                        if (bgWorker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }

                        list.Add(k.Name);
                    }
                    cat.AddCollection(unsortedTableModel.ExtractListByKeys(list));
                }
                bgWorker.ReportProgress(70, "Выполнена сортировка таблицы по ключам и категориям");

                // Корректировка номеров блоков данных для PID
                OneClickCorrectPiDs();

                foreach (var category in categories)
                {
                    if (bgWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    category.SortCollectionByCodename();
                    exWorks.PrintArrayToSheetTemplate(category.GetSymbolsArrayEx(), category.Name);
                }

                exWorks.PrintArrayToSheet(unsortedTableModel.GetSymbolsArray(), "unSorTed");

                bgWorker.ReportProgress(99, "Выполнена выгрузка категорий в Excel");

                e.Result = new BaseEntityModel(2, "Выполнено!", "Таблица  сигналов отсортирована и обработана");
            }
            else
            {
                MessageBox.Show("Не заданы категории для обработки таблицы сигналов!..");
                e.Result = new BaseEntityModel(2, "Не выполнено!", "Не заданы категории для обработки таблицы сигналов!..");
            }
        }


        /// <summary>
        /// Обработка коллекции списков сигналов и генерация текстов
        /// исходных кодов на языках низкого уровня для
        /// использования в программах ПЛК
        /// </summary>
        private void OneClickSourceGenerator(DoWorkEventArgs e)
        {
            sources = new SourceGenerator(categories.ToList());

            bgWorker.ReportProgress(30, "Выгрузка листа блоков данных...");
            exWorks.PrintArrayToSheet(sources.PrintDBlistToArray(), "DB_list");

            if (bgWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            bgWorker.ReportProgress(40, "Старт генерации source-файлов...");
            if ((G.sourcedir == null) | (G.sourcedir == ""))
                G.sourcedir = Environment.CurrentDirectory;
            sources.SetPeripheryFields();

            sources.PrintAllSourcesToFiles(G.sourcedir);

            bgWorker.ReportProgress(90, "Генерация source-файлов завершена");

            categories = new ObservableCollection<CategoryModel>(sources.Categories);

            e.Result = new BaseEntityModel(3, "Выполнено!", "Генерация завершена, основная структура сигналов обновлена");
        }


        private void OneClickCorrectPiDs()
        {
            if (categories.Count > 0)
                foreach (var cat in categories)
                {
                    if (cat.Id == 10000)
                    {
                        var sysN = 0;
                        var devN = 0;

                        foreach (var pid in cat.S7Items)
                        {
                            sysN = int.Parse(pid.SystemNumber);
                            devN = int.Parse(pid.DeviceNumber);
                            var n = (int)cat.Id++;
                            pid.DbFullName = "DB" + n;
                        }
                    }

                    if (cat.Id != 99) continue;
                    foreach (var sc in cat.S7Items)
                    {
                        sc.DbFullName = "DB99";

                    }
                }
        }


        /// <summary>
        /// Создание новой базы данных из коллекции объектов в памяти
        /// </summary>
        private void OneClickDbCreate()
        {
            //bgWorker.ReportProgress(10, "Начата генерация базы данных");

            if (categories != null)
            {
                var frameView = new MainFramesView(categories);

                //bgWorker.ReportProgress(90, "База данных создана");
                frameView.Show();
            }
            //else bgWorker.ReportProgress(100, "Не из чего создавать");

            //e.Result = new BaseEntityModel(4, "Выполнено!", "Генерация базы данных завершена");
        }


        /// <summary>
        /// Открыть существующую базу данных сигналов 
        /// из файла на диске
        /// </summary>
        private void OneClickDbOpen()
        {
            var dir = G.rootdir;
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


                G.DBfilename = filename;

                var frameView = new MainFramesView(filename);
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
            var about = new dialog_About();
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
            if (!bgWorker.IsBusy)
            {
                var arg = new BaseEntityModel();
                arg.Id = 4;
                arg.Name = "CreateDataBase";
                arg.Description = "Создание базы данных из коллекции объетов в памяти";
                bgWorker.RunWorkerAsync(arg);
            }
            else
                MessageBox.Show("Уже идет выполнение фоновой операции");
        }




        //----- Интерфейс второстепенных операций -----------------------------
        private void BtnDelListsClick(object sender, RoutedEventArgs e)
        {
            var catNames = new List<string>();

            foreach (var cat in categories)
            {
                catNames.Add(cat.Name);
            }
            exWorks.DeleteLists(catNames);
        }


        //----- Второстепенные и вспомогательные функции и операции
        public void Print2Result(string s)
        {
            TxtResult.AppendText("\r\n" + DateTime.Now.ToString("h:mm:ss") + ": " + s);
            TxtResult.ScrollToEnd();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (exWorks != null)
            {

                try
                {
                    exWorks.CloseExcel(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }

            }
        }



        //------ Тестирование кода -------------------------------------
        private void BtnTestCodeClick(object sender, RoutedEventArgs e)
        {
            //cBaseItem arg = new cBaseItem();
            //arg.ID = 1;
            //arg.Name = "Adaptation";
            //arg.Description = "Адаптация таблицы";

            //bgWorker.RunWorkerAsync(arg);

            sources = new SourceGenerator();
            sources.Rootdir = "E:\\7345\\sources\\PLC1";
            sources.MergePeripheryFiles();

        }

        private void BoxShowExcelClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtnS7CodeClick(object sender, RoutedEventArgs e)
        {
           // var s7 = new Step7_Works();

            //s7.S7testCode();

            viewModel.SerializeTableCommand.Execute(G.filename);
            
        }


    }
}
