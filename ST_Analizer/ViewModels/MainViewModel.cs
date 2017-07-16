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

        public ObservableCollection<string> ConsoleLines { get; set; }
        
        public ICommand SerializeTableCommand { get; set; }
        public ICommand CategoriesCommand { get; set; }
        public ICommand FileSelectCommand { get; set; }


        public MainViewModel()
        {
            Categories = new ObservableRangeCollection<CategoryModel>();
            SerializeTableCommand = new RelayCommand(async obj => await ExecuteSerializeTableCommand(RootDirectory));
            CategoriesCommand = new RelayCommand(obj =>
            {
                CategoriesView view = new CategoriesView(Categories);
                view.ShowDialog();
            });

            FileSelectCommand = new RelayCommand(obj =>
            {
                RootDirectory = FileManager.OpenFile(RootDirectory);
                ConsoleWrite("Задан файл конфигурации...");
            });

            ConsoleLines = new ObservableCollection<string>();
            ConsoleWrite("Initialized...");
            RootDirectory = Environment.CurrentDirectory;
        }

        public void ConsoleWrite(string line)
        {
            ConsoleLines.Add(Environment.NewLine + DateTime.Now.ToString("h:mm:ss") + ": " + line);
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
        }
    }
}
