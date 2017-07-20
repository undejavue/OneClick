using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ClassLibrary.Models;
using OneClickUI.Helpers;
using ClassLibrary.Excel;
using ClassLibrary.Services;
using ClassLibrary.SourceGenerator;
using OneClickUI.Log;
using OneClickUI.Views;

namespace OneClickUI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string _rootDirectory = string.Empty;
        public string RootDirectory
        {
            get { return _rootDirectory; }
            set { SetProperty(ref _rootDirectory, value); }
        }

        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }

        public SymbolTableModel SymbolTable { get; set; }

        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        public ObservableRangeCollection<CategoryModel> Categories { get; set; }
        public ObservableCollection<LogEntryModel> LogItems { get; set; }
        public ObservableCollection<LogTagModel> LogFIlter { get; set; }

        public ICommand SerializeTableCommand { get; set; }
        public ICommand CategorizeTableCommand { get; set; }
        public ICommand GenerateSourcesCommand { get; set; }
        public ICommand CategoriesCommand { get; set; }
        public ICommand FileSelectCommand { get; set; }
        public ICommand FilterLogCommand { get; set; }


        public MainViewModel()
        {
            Categories = new ObservableRangeCollection<CategoryModel>();

            SerializeTableCommand = new RelayCommand
                (async obj => await ExecuteSerializeTableCommand(RootDirectory,
                                    new Progress<int>(progress => Progress = progress)));

            CategorizeTableCommand = new RelayCommand(obj =>
                ExecuteCategorizeTableCommand(new Progress<int>(progress => Progress = progress)));

            CategoriesCommand = new RelayCommand(obj =>
            {
                CategoriesView view = new CategoriesView(Categories);
                view.ShowDialog();
                ConsoleWrite("Созданы категории сортировки...");
            });

            GenerateSourcesCommand = new RelayCommand(obj =>
                ExecuteGenerateSourcesCommand(new Progress<int>(progress => Progress = progress)));

            FileSelectCommand = new RelayCommand(obj =>
            {
                RootDirectory = FileManager.OpenFile(RootDirectory);
                ConsoleWrite("Задан файл конфигурации...");
            });

            FilterLogCommand = new RelayCommand(ExecuteFilterLogCommand);
            InitFilter();
            RootDirectory = Environment.CurrentDirectory;
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            ConsoleWrite("Initialized...");
        }

        private void InitFilter()
        {
            LogItems = new ObservableCollection<LogEntryModel>();
            LogFIlter = new ObservableCollection<LogTagModel>();
            LogFIlter.Add(new LogTagModel(LogTag.All));
            LogFIlter.Add(new LogTagModel(LogTag.Debug));
            LogFIlter.Add(new LogTagModel(LogTag.Error));
            LogFIlter.Add(new LogTagModel(LogTag.Info));
            LogFIlter.Add(new LogTagModel(LogTag.Warning));

        }

        private void ExecuteFilterLogCommand(object obj)
        {
            var filter = obj as string;
            if (filter != null && filter.Equals(LogTag.All.ToString()))
            {
                foreach (var filterItem in LogFIlter)
                {
                    filterItem.IsSelected = true;
                }
            }
            else
            {
                foreach (var filterItem in LogFIlter)
                {
                    filterItem.IsSelected = filterItem.Name.Equals(filter);
                }
            }

            foreach (var item in LogItems)
            {
                if (LogFIlter.Any(x => x.Id == item.Tag.Id && x.IsSelected))
                    item.IsVisible = Visibility.Visible;
                else
                    item.IsVisible = Visibility.Hidden;
            }
        }

        public void ConsoleWrite(string line, LogTag tag = LogTag.Debug)
        {
            LogItems.Add(new LogEntryModel(tag, line));
        }

        private async Task ExecuteSerializeTableCommand(object param, IProgress<int> progress)
        {
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            await Task.Run(() =>
            {
                var filename = (string)param;
                var dt = OneService.GenerateTableFromExcel(filename);

                progress.Report(30);

                IList<SymbolTableItemModel> items = dt.AsEnumerable()
                        .Select(row =>
                            new SymbolTableItemModel
                            {
                                SignalName = row.Field<string>("SignalName"),
                                SignalAdress = row.Field<string>("SignalAdress"),
                                SignalDataType = row.Field<string>("SignalDataType"),
                                SignalComment = row.Field<string>("SignalComment")
                            })
                        .ToList();

                SymbolTable = new SymbolTableModel(items);
                SymbolTable.AnalyseAndSetTags();
                SymbolTable.SortTable();
                //var arr = SymbolTable.GetSymbolsArray();
                ExcelDataWriter.BackupFile(filename);
                ExcelDataWriter.WriteExcelFromArray(filename, SymbolTable.GetSymbolsArray(), "SymbolTable");

            }, token).ConfigureAwait(false);

            ConsoleWrite("Table is serialized", LogTag.Info);
        }


        private void ExecuteCategorizeTableCommand(IProgress<int> progress)
        {
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            ConsoleWrite("Запуск цикла сортировки таблицы по категориям...", LogTag.Info);
            int count = Categories.Count;

            if (count > 0)
            {

                foreach (var cat in Categories)
                {
                    var list = cat.Keys.Select(k => k.Name).ToList();
                    cat.AddCollection(SymbolTable.ExtractListByKeys(list));


                    progress.Report(count--);
                    ConsoleWrite($"Осталось категорий: {count}", LogTag.Info);
                }


                foreach (var cat in Categories)
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


                ConsoleWrite("Выполнена сортировка таблицы по ключам и категориям", LogTag.Info);

                var resultFile = Path.Combine(RootDirectory, "result.xls");
                ExcelDataWriter.WriteExcel(resultFile, Categories);

                resultFile = Path.Combine(RootDirectory, "result_unsorted.xls");
                ExcelDataWriter.WriteExcelFromArray(resultFile, SymbolTable.GetSymbolsArray(), "unsorted");

                ConsoleWrite("Выполнена выгрузка категорий в Excel", LogTag.Info);
                ConsoleWrite("Выполнено! Таблица  сигналов отсортирована и обработана", LogTag.Info);
            }
            else
            {
                ConsoleWrite("Не заданы категории для обработки таблицы сигналов!..", LogTag.Error);
            }
        }

        private void ExecuteGenerateSourcesCommand(IProgress<int> progress)
        {
            var sources = new SourceGenerator(Categories);

            ConsoleWrite("Выгрузка листа блоков данных...", LogTag.Info);

            var resultFile = Path.Combine(RootDirectory, "sources.xls");
            ExcelDataWriter.WriteExcelFromArray(resultFile, sources.PrintDBlistToArray(), "DB_list");

            progress.Report(40);
            ConsoleWrite("Старт генерации source-файлов...", LogTag.Info);
            sources.SetPeripheryFields();

            //sources.PrintAllSourcesToFiles(G.sourcedir);
            progress.Report(90);
            ConsoleWrite("Генерация source-файлов завершена", LogTag.Info);

            Categories.ReplaceRange(sources.Categories);

            progress.Report(100);
            ConsoleWrite("Выполнено! Генерация завершена, основная структура сигналов обновлена", LogTag.Info);
        }
    }
}
