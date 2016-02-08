using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using ClassLibrary;

using Microsoft.Win32;


//using SimaticLib;
//using S7HCOM_XLib;


namespace OneClick_Analyser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<mCategory> categories;
        private SourceGenerator Sources;
        private ExcelWorks ExWorks;
        private dialog_categories wnd_Categories;
        private BackgroundWorker asyncOperations;

        // Globals properties class
        public Globals G;

        public MainWindow()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            categories = new ObservableCollection<mCategory>();


            // Acync worker for continuous operation
            asyncOperations = new BackgroundWorker();
            asyncOperations.DoWork += asyncOperations_DoWork;
            asyncOperations.RunWorkerCompleted += asyncOperations_RunWorkerCompleted;
            asyncOperations.ProgressChanged += asyncOperations_ProgressChanged;
            asyncOperations.WorkerReportsProgress = true;

            //txt_filename.DataContext = global_FileName;

            OneClick_setGlobals();

        }

        //----- Асинхронное выполнение основных операций ------------------------------

        private void asyncOperations_DoWork(object sender, DoWorkEventArgs e)
        {
            mBaseEntity arg = (mBaseEntity)e.Argument;

            switch (arg.Id)
            {
                case 1:
                    OneClick_TableAdaptation(e);
                    break;
                case 2:
                    OneClick_TableToCollection(e);
                    break;
                case 3:
                    OneClick_SourceGenerator(e);
                    break;
                case 4:
                    //OneClick_DB_Create(e);
                    break;
                default:
                    break;
            }
        }

        private void asyncOperations_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
            this.txt_result.AppendText("\r\n" + DateTime.Now.ToString("h:mm:ss") + ": " + e.UserState.ToString());
            this.txt_result.ScrollToEnd();

            label_Process.Content = "Выполняется...";
        }

        private void asyncOperations_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Операция отменена", "S7 analyzer", MessageBoxButton.OK);
                label_Process.Content = "Отменено!";
                print2result("Операция отменена");
            }
            else
            {
                mBaseEntity result = (mBaseEntity)e.Result;
                print2result(result.Description);
                label_Process.Content = result.Name;
            }

            progressBar.IsIndeterminate = false;
            this.progressBar.Value = 100;

            btn_Cancel.IsEnabled = false;
            btn_GenSource.IsEnabled = true;

            ExWorks.setVisible(true);
        }

        //----- Интерфейс основных операций  -----------------------------------------


        //--- Конфигурация опций интерфейса
        private void OneClick_setGlobals()
        {
            G = new Globals();

            G.rootdir = "D:\\OneClickDB";

            gridFileOperations.DataContext = G;
            gridDBOperations.DataContext = G;

        }


        //--- Загрузка файла конфигурации
        private void btn_ExcelOpen_Click(object sender, RoutedEventArgs e)
        {
            if (ExWorks == null)
            {
                ExWorks = new ExcelWorks(G.rootdir);
                ExWorks.ReportMessage += ExWorks_ReportMessage;
            }

            G.rootdir = ExWorks.OpenExcel();

            btn_TblAdapt.IsEnabled = true;
            btn_Operations.IsEnabled = true;
            btn_GenSource.IsEnabled = true;
            btn_Save.IsEnabled = true;
            btn_Close.IsEnabled = true;

            G.filename = ExWorks.fileName;
        }

        private void ExWorks_ReportMessage(object sender, OneClickEventArgs args)
        {
            if (asyncOperations.IsBusy)
            {
                asyncOperations.ReportProgress(50, args.message);
            }
            else
            {
                this.txt_result.AppendText("\r\n" + DateTime.Now.ToString("h:mm:ss") + ": " + args.message);
            }
        }


        //--- Первоначальная обработка таблицы
        private void btn_TableAdaptation_Click(object sender, RoutedEventArgs e)
        {
            if (!asyncOperations.IsBusy)
            {
                mBaseEntity arg = new mBaseEntity();
                arg.Id = 1;
                arg.Name = "Analyse";
                arg.Description = "Анализ таблицы сигналов";
                asyncOperations.RunWorkerAsync(arg);

                btn_Cancel.IsEnabled = true;
                ExWorks.setVisible(G.isExcelVisible);
            }
            else
                MessageBox.Show("Уже идет выполнение фоновой операции");
        }

        //--- Задание категорий для коллекции
        private void btn_setCategories_Click(object sender, RoutedEventArgs e)
        {
            wnd_Categories = new dialog_categories(categories);
            wnd_Categories.Show();
            G.categoriesCount = categories.Count;

            wnd_Categories.btn_SaveChanges.Click += wnd_Categories_SaveChanges_Click;
        }

        private void wnd_Categories_SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            categories = new ObservableCollection<mCategory>(wnd_Categories.categories);
            G.categoriesCount = categories.Count;
        }


        //--- Создание коллекции из таблицы и работа с ней
        private void btn_Operations_Click(object sender, RoutedEventArgs e)
        {
            if (!asyncOperations.IsBusy)
            {
                mBaseEntity arg = new mBaseEntity();
                arg.Id = 2;
                arg.Name = "Categorize";
                arg.Description = "Сортировка сигналов по таблицам категорий";
                asyncOperations.RunWorkerAsync(arg);

                btn_Cancel.IsEnabled = true;
                ExWorks.setVisible(G.isExcelVisible);
            }
            else
                MessageBox.Show("Уже идет выполнение фоновой операции");
        }

        //--- Генерация файлов исходного кода для PLC-программы
        private void btn_GenSource_Click(object sender, RoutedEventArgs e)
        {
            if (!asyncOperations.IsBusy)
            {
                mBaseEntity arg = new mBaseEntity();
                arg.Id = 3;
                arg.Name = "GenerateSources";
                arg.Description = "Генерация текстов исходных кодов";
                asyncOperations.RunWorkerAsync(arg);
            }
            else
                MessageBox.Show("Уже идет выполнение фоновой операции");
        }

        //--- Отмена выполнения фоновых операций
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            asyncOperations.CancelAsync();
        }

        //--- Сохранить и закрыть файл конфигурации
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            ExWorks.closeExcel(true);
            G.filename = "";
        }

        //--- Закрыть файл конфигурации
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            ExWorks.closeExcel(false);
            G.filename = "";
        }

        //--- Создать базу данных
        private void btn_DBcreate_Click(object sender, RoutedEventArgs e)
        {
            
            //if (!asyncOperations.IsBusy)
            //{
            //    mBaseEntity arg = new mBaseEntity();
            //    arg.Id = 4;
            //    arg.Name = "CreateDataBase";
            //    arg.Description = "Создание базы данных из коллекции объетов в памяти";
            //    asyncOperations.RunWorkerAsync(arg);
            //}
            //else
            //    MessageBox.Show("Уже идет выполнение фоновой операции");
            label_Process.Content = "Выполняется...";
            OneClick_DB_Create();
            label_Process.Content = "Готово";
        }

        //--- Открыть базу данных
        private void btn_DBopen_Click(object sender, RoutedEventArgs e)
        {
            OneClick_DB_Open();
        }


        //----- Основные функции --------------------------------------------------


        /// <summary>
        /// Анализ исходной таблицы символов, формирование дополнительных полей таблицы
        /// из символьного имени сигнала и комментария, сортировка таблицы
        /// </summary>
        /// <param name="e">Параметры асинхронного обработчика</param>
        private void OneClick_TableAdaptation(DoWorkEventArgs e)
        {
            ExWorks.excel_backupSheet("SymbolTable");
            asyncOperations.ReportProgress(5, "Резервная копия таблицы создана");

            cSymbolTable symbolTable = new cSymbolTable(ExWorks.generate_ArrayFromRange("SymbolTable", true));
            asyncOperations.ReportProgress(30, "Таблица прочитана в память");

            symbolTable.analyseAndSetTags();
            asyncOperations.ReportProgress(60, "Анализ символьных имен выполнен");

            symbolTable.sortTable();
            asyncOperations.ReportProgress(70, "Сортировка выполнена");

            String[,] arr = symbolTable.return_ArrayOfSymbols();
            ExWorks.printArrayToSheet(arr, "SymbolTable");
            asyncOperations.ReportProgress(98, "Таблица выгружена в Excel");

            symbolTable.clearSymbols();

            e.Result = new mBaseEntity(1, "Выполнено!", "Первичная обработка таблицы выполнена");
        }

        /// <summary>
        /// Формирование в памяти коллекции символьных списков,
        /// из исходной таблицы по заданным категориям и ключам
        /// </summary>
        /// <param name="e"></param>
        private void OneClick_TableToCollection(DoWorkEventArgs e)
        {
            ExWorks.excel_backupSheet("SymbolTable");
            asyncOperations.ReportProgress(10, "Запуск цикла сортировки таблицы по категориям...");

            if (categories.Count > 0)
            {
                cSymbolTable unsorted_table = new cSymbolTable(ExWorks.generate_ArrayFromRange("SymbolTable", true));
                asyncOperations.ReportProgress(40, "Таблица загружена в память");


                foreach (mCategory cat in categories)
                {
                    List<string> list = new List<string>();
                    foreach (mBaseEntity k in cat.Keys)
                    {
                        list.Add(k.Name);
                    }
                    cat.addCollection(unsorted_table.extractListByKeys(list));
                }
                asyncOperations.ReportProgress(70, "Выполнена сортировка таблицы по ключам и категориям");

                // Корректировка номеров блоков данных для PID
                OneClick_CorrectPIDs();

                foreach (mCategory category in categories)
                {

                    category.sortCollectionByCodename();
                    ExWorks.printArrayToSheetTemplate(category.return_ArrayOfSymbolsEX(), category.Name);
                }

                ExWorks.printArrayToSheet(unsorted_table.return_ArrayOfSymbols(), "unSorTed");

                asyncOperations.ReportProgress(99, "Выполнена выгрузка категорий в Excel");

                e.Result = new mBaseEntity(2, "Выполнено!", "Таблица  сигналов отсортирована и обработана");
            }
            else
            {
                MessageBox.Show("Не заданы категории для обработки таблицы сигналов!..");
                e.Result = new mBaseEntity(2, "Не выполнено!", "Не заданы категории для обработки таблицы сигналов!..");
            }
        }


        /// <summary>
        /// Обработка коллекции списков сигналов и генерация текстов
        /// исходных кодов на языках низкого уровня для
        /// использования в программах ПЛК
        /// </summary>
        private void OneClick_SourceGenerator(DoWorkEventArgs e)
        {
            Sources = new SourceGenerator(categories.ToList());

            asyncOperations.ReportProgress(30, "Выгрузка листа блоков данных...");
            ExWorks.printArrayToSheet(Sources.printDBlistToArray(), "DB_list");

            asyncOperations.ReportProgress(40, "Старт генерации source-файлов...");
            if ((G.sourcedir == null) | (G.sourcedir == "")) G.sourcedir = Environment.CurrentDirectory;
            Sources.set_PeripheryFields();

            Sources.printAllSourcesToFiles(G.sourcedir);

            asyncOperations.ReportProgress(90, "Генерация source-файлов завершена");

            categories = new ObservableCollection<mCategory>(Sources.categories);

            e.Result = new mBaseEntity(3, "Выполнено!", "Генерация завершена, основная структура сигналов обновлена");
        }


        private void OneClick_CorrectPIDs()
        {
            if (categories.Count > 0)
                foreach (mCategory cat in categories)
                {
                    if (cat.Id == 10000)
                    {
                        int sysN = 0;
                        int devN = 0;
                        int N = 0;
                        string s = "";
                        foreach (mSymbolTableItem pid in cat.S7items)
                        {
                            sysN = int.Parse(pid.SystemNumber);
                            devN = int.Parse(pid.DeviceNumber);
                            N = (int)cat.Id++;
                            pid.DB_FullName = "DB" + N.ToString();

                        }
                        //break;
                    }

                    if (cat.Id == 99)
                    {
                        foreach (mSymbolTableItem sc in cat.S7items)
                        {
                            sc.DB_FullName = "DB99";

                        }
                    }

                }
        }


        /// <summary>
        /// Создание новой базы данных из коллекции объектов в памяти
        /// </summary>
        private void OneClick_DB_Create()
        {
            //asyncOperations.ReportProgress(10, "Начата генерация базы данных");

            if (categories != null)
            {
                MainFrames frame = new MainFrames(categories);

                //asyncOperations.ReportProgress(90, "База данных создана");
                frame.Show();
            }
            //else asyncOperations.ReportProgress(100, "Не из чего создавать");

            //e.Result = new mBaseEntity(4, "Выполнено!", "Генерация базы данных завершена");
        }


        /// <summary>
        /// Открыть существующую базу данных сигналов 
        /// из файла на диске
        /// </summary>
        private void OneClick_DB_Open()
        {
            string dir = G.rootdir;
            string filename = "";

            OpenFileDialog openDlg = new OpenFileDialog();

            openDlg.InitialDirectory = dir;
            openDlg.Filter = "Database files (*.mdf)|*.mdf;|All Files (*.*)|*.*";

            // Set filter for file extension and default file extension
            openDlg.DefaultExt = ".mdf";

            // Display OpenFileDialog by calling ShowDialog method
            bool? result = openDlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                string safename = openDlg.SafeFileName;
                filename = openDlg.FileName;

                dir = filename.Remove(filename.Length - safename.Length - 1);


                G.DBfilename = filename;

                MainFrames frame = new MainFrames(filename);
                frame.Show();
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
            dialog_About about = new dialog_About();
            about.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_OpenDB_Click(object sender, RoutedEventArgs e)
        {
            OneClick_DB_Open();

        }

        private void MenuItem_CreateDB_Click(object sender, RoutedEventArgs e)
        {
            if (!asyncOperations.IsBusy)
            {
                mBaseEntity arg = new mBaseEntity();
                arg.Id = 4;
                arg.Name = "CreateDataBase";
                arg.Description = "Создание базы данных из коллекции объетов в памяти";
                asyncOperations.RunWorkerAsync(arg);
            }
            else
                MessageBox.Show("Уже идет выполнение фоновой операции");
        }




        //----- Интерфейс второстепенных операций -----------------------------
        private void btn_DelLists_Click(object sender, RoutedEventArgs e)
        {
            List<string> catNames = new List<string>();

            foreach (mCategory cat in categories)
            {
                catNames.Add(cat.Name);
            }
            ExWorks.deleteLists(catNames);
        }


        //----- Второстепенные и вспомогательные функции и операции
        public void print2result(string s)
        {
            txt_result.AppendText("\r\n" + DateTime.Now.ToString("h:mm:ss") + ": " + s);
            txt_result.ScrollToEnd();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (ExWorks != null)
            {

                try
                {
                    ExWorks.closeExcel(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }

            }
        }



        //------ Тестирование кода -------------------------------------
        private void btn_TestCode_Click(object sender, RoutedEventArgs e)
        {
            //cBaseItem arg = new cBaseItem();
            //arg.ID = 1;
            //arg.Name = "Adaptation";
            //arg.Description = "Адаптация таблицы";

            //asyncOperations.RunWorkerAsync(arg);

            Sources = new SourceGenerator();
            Sources.rootdir = "E:\\7345\\sources\\PLC1";
            Sources.mergePeripheryFiles();

        }

        private void box_ShowExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_S7Code_Click(object sender, RoutedEventArgs e)
        {
            Step7_Works s7 = new Step7_Works();

            s7.S7testCode();
        }


    }
}
