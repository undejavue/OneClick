using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
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
        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        private string _rootDirectory = string.Empty;
        public string RootDirectory
        {
            get { return _rootDirectory; }
            set { SetProperty(ref _rootDirectory, value); }
        }

        private string _filePath = string.Empty;
        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
        }

        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }

        private string _progressLabel;
        public string ProgressLabel
        {
            get { return _progressLabel; }
            set { SetProperty(ref _progressLabel, value); }
        }

        public SymbolTableModel SymbolTable { get; set; }
        public ObservableRangeCollection<CategoryModel> Categories { get; set; }
        public ObservableCollection<LogEntryModel> LogItems { get; set; }
        public ObservableCollection<LogTagModel> LogFIlter { get; set; }

        public ICommand SerializeTableCommand { get; set; }
        public ICommand CategorizeTableCommand { get; set; }
        public ICommand GenerateSourcesCommand { get; set; }
        public ICommand CategoriesCommand { get; set; }
        public ICommand FileSelectCommand { get; set; }
        public ICommand FilterLogCommand { get; set; }
        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ??
                    (_cancelCommand = new RelayCommand(obj => tokenSource?.Cancel()));
            }
        }

        private static readonly object SyncLock = new object();

        public MainViewModel()
        {
            Categories = new ObservableRangeCollection<CategoryModel>();
            Task.Run(SetCategories);

            SerializeTableCommand = new RelayCommand(
                async obj => await ExecuteSerializeTableCommand());

            CategorizeTableCommand = new RelayCommand(
                async obj => await ExecuteCategorizeTableCommand());

            CategoriesCommand = new RelayCommand(obj =>
            {
                var view = new CategoriesView(Categories);
                view.ShowDialog();
                ConsoleWrite("Созданы категории сортировки...");
            });

            GenerateSourcesCommand = new RelayCommand(
                async obj => await ExecuteGenerateSourcesCommand());

            FileSelectCommand = new RelayCommand(obj =>
            {
                FilePath = FileManager.OpenFile(RootDirectory);
                RootDirectory = new FileInfo(FilePath).DirectoryName;
                ConsoleWrite("Задан файл конфигурации...");
            });

            FilterLogCommand = new RelayCommand(ExecuteFilterLogCommand);
            InitFilter();
            RootDirectory = Environment.CurrentDirectory;
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            ConsoleWrite("Initialized...");
            BindingOperations.EnableCollectionSynchronization(LogItems, SyncLock);
        }

        private async Task SetCategories()
        {
            var defaultList = await OneService.GenerateDefaultCategoriesAsync();
            if (Categories == null)
                Categories = new ObservableRangeCollection<CategoryModel>(defaultList);
            else
            {
                Categories.ReplaceRange(defaultList);
            }
            ConsoleWrite("Созданы категории сортировки...");
        }

        private void InitFilter()
        {
            LogItems = new ObservableCollection<LogEntryModel>();
            LogFIlter = new ObservableCollection<LogTagModel>
            {
                new LogTagModel(LogTag.All),
                new LogTagModel(LogTag.Debug),
                new LogTagModel(LogTag.Error),
                new LogTagModel(LogTag.Info),
                new LogTagModel(LogTag.Warning)
            };
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

        public void ConsoleWrite(string line, LogTag tag = LogTag.Info)
        {
            LogItems.Add(new LogEntryModel(tag, line));
        }

        private async Task ExecuteSerializeTableCommand()
        {
            if (!File.Exists(FilePath))
            {
                ConsoleWrite("Ошибка открытия файла", LogTag.Error);
                return;
            }              

            IsBusy = true;
            ProgressLabel = "Выполняется...";

            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            await Task.Run(() =>
            {
                var dt = OneService.GenerateTableFromExcel(FilePath);
                if (dt == null)
                {
                    lock (SyncLock)
                    {
                        ConsoleWrite("Ошибка открытия файла", LogTag.Error);
                        return;
                    }
                }
                Progress = 30;

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

                lock (SyncLock) { ConsoleWrite("Таблица прочитана"); }
                if (token.IsCancellationRequested)
                {
                    lock (SyncLock) { ConsoleWrite("Операция отменена"); }
                    return;
                }

                SymbolTable.AnalyseAndSetTags();
                SymbolTable.SortTable();

                lock (SyncLock) { ConsoleWrite("Анализ таблицы выполнен"); }
                Progress = 50;

                ExcelDataWriter.BackupFile(FilePath);

                var fi = new FileInfo(FilePath);
                var dest = Path.Combine(RootDirectory, "result_" + fi.Name);

                bool isOk = ExcelDataWriter.WriteExcelFromArray(dest, SymbolTable.GetSymbolsArray(), "SymbolTable", token);
                Progress = 100;
                var s = isOk ? "Адаптированная таблица сохранена" : "Ошибка записи файла";
                lock (SyncLock){ ConsoleWrite(s); }

            }, token).ConfigureAwait(false);

            IsBusy = false;
            ProgressLabel = "Завершено";
        }


        private async Task ExecuteCategorizeTableCommand()
        {
            IsBusy = true;
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            ConsoleWrite("Запуск цикла сортировки таблицы по категориям...");


            int count = Categories.Count;
            if (count == 0)
            {
                ConsoleWrite("Не заданы категории для обработки таблицы сигналов!..", LogTag.Error);
                return;
            }

            await Task.Run(() =>
            {
                ProgressLabel = "Выгрузка категорий";
                int i = 1;

                foreach (var cat in Categories)
                {
                    var list = cat.Keys.Select(k => k.Name).ToList();
                    cat.AddCollection(SymbolTable.ExtractListByKeys(list));

                    Progress = (int)((double)Categories.Count / 100 * i++);
                    lock (SyncLock)
                    {
                        ConsoleWrite($"Осталось категорий: {count--}");
                    }
                }

                ProgressLabel = "Уточнение типов";
                i = 1;

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
                    Progress = (int)((double)Categories.Count / 100 * i++);
                }

                lock (SyncLock)
                {
                    ConsoleWrite("Выполнена сортировка таблицы по ключам и категориям");
                }

                var resultFile = Path.Combine(RootDirectory, "result.xls");
                bool isOk = ExcelDataWriter.WriteExcel(resultFile, Categories);

                var message = isOk ? "Выполнена выгрузка категорий в Excel" : "Ошибка записи файла";
                lock (SyncLock)
                {
                    ConsoleWrite(message);
                }

                resultFile = Path.Combine(RootDirectory, "result_unsorted.xls");
                isOk = ExcelDataWriter.WriteExcelFromArray(resultFile, SymbolTable.GetSymbolsArray(), "unsorted", token);

            }, token).ConfigureAwait(false);

            ProgressLabel = "Выполнено";
            Progress = 0;
            ConsoleWrite("Выполнено! Таблица  сигналов отсортирована и обработана");
            IsBusy = false;
        }

        private async Task ExecuteGenerateSourcesCommand()
        {
            IsBusy = true;
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            var sources = new SourceGenerator(Categories);

            ConsoleWrite("Выгрузка листа блоков данных...");

            var resultFile = Path.Combine(RootDirectory, "sources.xls");
            await Task.Run(() =>
            {
                ExcelDataWriter.WriteExcelFromArray(resultFile, sources.PrintDBlistToArray(), "DB_list", token);
            }, token).ConfigureAwait(false);

            Progress = 40;
            ConsoleWrite("Старт генерации source-файлов...");
            await sources.SetPeripheryFields(token);

            Progress = 90;
            ConsoleWrite("Генерация source-файлов завершена");

            Categories.ReplaceRange(sources.Categories);

            Progress = 0;
            IsBusy = false;
            ConsoleWrite("Выполнено! Генерация завершена, основная структура сигналов обновлена");
        }
    }
}
