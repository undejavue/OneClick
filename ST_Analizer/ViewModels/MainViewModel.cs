using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ClassLibrary.Models;
using OneClickUI.Helpers;
using ClassLibrary.Excel;
using ClassLibrary.Services;
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

        public SymbolTableModel SymbolTable { get; set; }

        public ObservableRangeCollection<CategoryModel> Categories { get; set; }

        public ObservableCollection<LogEntryModel> LogItems { get; set; }

        public ObservableCollection<LogTagModel> LogFIlter { get; set; }

        public ICommand SerializeTableCommand { get; set; }
        public ICommand CategoriesCommand { get; set; }
        public ICommand FileSelectCommand { get; set; }
        public ICommand FilterLogCommand { get; set; }


        public MainViewModel()
        {
            Categories = new ObservableRangeCollection<CategoryModel>();

            SerializeTableCommand = new RelayCommand(async obj => await ExecuteSerializeTableCommand(RootDirectory));
            CategoriesCommand = new RelayCommand(obj =>
            {
                CategoriesView view = new CategoriesView(Categories);
                view.ShowDialog();
                ConsoleWrite("Созданы категории сортировки...");
            });

            FileSelectCommand = new RelayCommand(obj =>
            {
                RootDirectory = FileManager.OpenFile(RootDirectory);
                ConsoleWrite("Задан файл конфигурации...");
            });

            FilterLogCommand = new RelayCommand(ExecuteFilterLogCommand);

            LogItems = new ObservableCollection<LogEntryModel>();
            LogFIlter = new ObservableCollection<LogTagModel>();
            InitFilter();
            ConsoleWrite("Initialized...");
            RootDirectory = Environment.CurrentDirectory;
        }

        private void InitFilter()
        {
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
                item.IsVisible = LogFIlter.Count <= 0 || LogFIlter.Where(x=>x.IsSelected).Contains(item.Tag);
            }
        }

        public void ConsoleWrite(string line)
        {
            LogItems.Add(new LogEntryModel(LogTag.Debug, line));
        }

        private async Task ExecuteSerializeTableCommand(object param)
        {

            await Task.Run(() =>
            {
                var filename = (string)param;
                var dt = OneService.GenerateTableFromExcel(filename);
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
                var arr = SymbolTable.GetSymbolsArray();
                ExcelDataWriter.BackupFile(filename);
                ExcelDataWriter.WriteExcelFromArray(filename, SymbolTable.GetSymbolsArray(), "SymbolTable");

            }).ConfigureAwait(false);

            ConsoleWrite("Table is serialized");
        }
    }
}
