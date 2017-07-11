using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ClassLibrary.Models
{
    public class CategoryModel : BaseEntityModel
    {
        /*
            cat.Description = "Дискретные сигналы";
            cat.DB.UDT_Number = "2";
            cat.DB.Symbol = "B";
            cat.DB.ArrayName = "SNB";
            cat.DB.UDT_Name = "SNB_UDT";
            cat.FCname = "periphery_SNB";
            cat.Keys.Add("атчик");
            cat.Keys.Add("фланш-панел");
            cat.Keys.Add("Датчик ФП");
            cat.Keys.Add("калача");
            cat.Keys.Add("Соединение");
        */
        public string FCname { get; set; }

        public DataBlockModel Db { get; set; }
        //{
        //    get { return DB; }
        //    set
        //    {
        //        DB = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("DB"));
        //    }
        //}

        private string _selectedKey;
        public string SelectedKey
        {
            get { return _selectedKey; }
            set
            {
                _selectedKey = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedKey)));
            }
        }

        /// <summary>
        /// Link to model mKeys, OneToMany
        /// </summary>
        public ObservableCollection<KeyModel> Keys { get; set; }

        /// <summary>
        /// Link to model SymbolTableItemModel, OneToMany
        /// </summary>
        //[ForeignKey("S7itemId")]
        public ObservableCollection<SymbolTableItemModel> S7Items { get; set; }

        public CategoryModel()
        {
            SelectedKey = null;
            Db = new DataBlockModel();
            S7Items = new ObservableCollection<SymbolTableItemModel>();
            Keys = new ObservableCollection<KeyModel>();
        }

        public CategoryModel(int initId, string initName, string initDescription)
        {
            Name = initName;
            Description = initDescription;
            SelectedKey = null;
            Db = new DataBlockModel();
            S7Items = new ObservableCollection<SymbolTableItemModel>();
            Keys = new ObservableCollection<KeyModel>();
        }


        public void AddDb(DataBlockModel newDb)
        {
            Db = newDb;

        }

        public void AddCollection(List<SymbolTableItemModel> newList)
        {
            foreach (var item in newList)
            {
                item.DbArrayName = Db.ArrayName;
                item.DbFullName = "DB" + item.SystemNumber + Db.UdtNumber;
                S7Items.Add(item);
            }
        }

        public void SortCollectionByCodename()
        {
            S7Items = new ObservableCollection<SymbolTableItemModel>(S7Items
                .OrderBy(key => key.Codename));
        }

        /// <summary>
        /// Возвращает массив для выгрузки в Excel, состоящий из элементов коллекции s7items
        /// </summary>
        /// <returns>array[s7items.count, 8]</returns>
        public string[,] GetSymbolsArrayEx()
        {
            var list = new List<string>();
            if (S7Items.Count > 0)
            {
                list = S7Items[0].GetItemInOneRow();
            }

            var arr = new string[S7Items.Count, list.Count];
            int i = 0;

            foreach (var el in S7Items)
            {
                list = el.GetItemInOneRow();
                int j = 0;
                foreach (string s in list)
                {
                    arr[i, j] = s;
                    j++;
                }
                i++;
            }

            return arr;
        }

        public bool IsEmpty()
        {
            var isEmpty = !(S7Items.Count > 0);
            return isEmpty;
        }

    }
}
