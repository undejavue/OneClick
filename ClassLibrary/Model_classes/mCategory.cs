using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    public partial class mCategory : mBaseEntity
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
        //{
        //    get { return FCname; }
        //    set
        //    {
        //        FCname = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("FCName"));
                
        //    }
        //}



        public virtual mDataBlock DB { get; set; }
        //{
        //    get { return DB; }
        //    set
        //    {
        //        DB = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("DB"));
        //    }
        //}


// !!!!!!!!!!!!!!!!!!!-------- > убрать???
        private string _SelectedKey;
        public string SelectedKey
        {
            get { return _SelectedKey; }
            set
            {
                _SelectedKey = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedKey"));
            }
        }

        /// <summary>
        /// Link to model mKeys, OneToMany
        /// </summary>
        public virtual ObservableCollection<mKey> Keys
        {
            get;
            set;
            //{
            //    _keys = value;
            //    OnPropertyChanged(new PropertyChangedEventArgs("Keys"));
            //}
        }

        /// <summary>
        /// Link to model SymbolTableItem, OneToMany
        /// </summary>
        //[ForeignKey("S7itemId")]
        public virtual ObservableCollection<mSymbolTableItem> S7items
        {
            get;
            set;
            //{
            //    S7items = value;
            //    OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, "S7items"));
            //}
        }



        public mCategory()
        {
            Keys = new ObservableCollection<mKey>();
            SelectedKey = null;
            S7items = new ObservableCollection<mSymbolTableItem>();
            DB = new  mDataBlock();
        }

        public mCategory(int initID, string initName, string initDescription)
        {
            Name = initName;
            Description = initDescription;
            Keys = new ObservableCollection<mKey>();
            SelectedKey = null;
            S7items = new ObservableCollection<mSymbolTableItem>();
            DB = new mDataBlock();
        }

    }
}
