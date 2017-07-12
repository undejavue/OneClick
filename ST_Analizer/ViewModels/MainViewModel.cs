using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ClassLibrary.Models;
using OneClickUI.Helpers;
using System.Data.OleDb;
using System.Diagnostics;
using System.Threading;
using ClassLibrary.Services;

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

        public ICommand SerializeTableCommand;


        public MainViewModel()
        {
            SerializeTableCommand = new RelayCommand(async obj => await ExecuteSerializeTableCommand(obj));
        }

        private async Task ExecuteSerializeTableCommand(object param)
        {
            
                await Task.Run(() =>
                 {
                     var dt = OneService.GenerateTableFromExcel((string)param);
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
                     
                 }).ConfigureAwait(false);
        }
    }
}
