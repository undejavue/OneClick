using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ClassLibrary
{
    public partial class mCategory
    {

      
        public void addDB(mDataBlock newDB)
        {
            DB = newDB;
        }
       
        public void addCollection(List<mSymbolTableItem> new_list)
        {
            foreach (mSymbolTableItem item in new_list)
            {
                item.DB_ArrayName = this.DB.ArrayName;
                item.DB_FullName = "DB" + item.SystemNumber + this.DB.UDT_Number;
                S7items.Add(item);
            }

            
        }

        public void sortCollectionByCodename()
        {
            this.S7items = new ObservableCollection<mSymbolTableItem>(this.S7items.OrderBy(key => key.Codename));
        }

        /// <summary>
        /// Возвращает массив для выгрузки в Excel, состоящий из элементов коллекции s7items
        /// </summary>
        /// <returns>array[s7items.count, 8]</returns>
        public String[,] return_ArrayOfSymbolsEX()
        {
            List<string> list = new List<string>();
            if (S7items.Count > 0)
            {
                list = S7items[0].return_ItemInOneRow();
            }

            var arr = new string[this.S7items.Count, list.Count];
            int i = 0;

            foreach (mSymbolTableItem el in S7items)
            {
                list = el.return_ItemInOneRow();
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

        public bool isEmpty()
        {
            bool isEmpty = true;

            if (this.S7items.Count > 0) isEmpty = false;

            return isEmpty;
        }


    }
}
