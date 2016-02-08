using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace ClassLibrary
{
    /// <summary>
    /// Элемент символьной таблицы
    /// </summary>
    public partial class mSymbolTableItem
    {

        //private int? _ID;                 // row# in table
        //private string _SignalName;       // #1  
        //private string _SignalAdress;     // #2
        //private string _SignalDataType;   // #3
        //private string _SignalComment;    // #4
        //private string _SignalType;       // #5
        //private string _Codename;         // #6
        //private string _SystemNumber;     // #7
        //private string _DeviceType;       // #8     
        //private string _DeviceNumber;     // #9
        //private string _Etc;              // #10
        //private string _DeviceTag;        // #11


        /// <summary>
        /// Для каждого созданного элемента сразу задаются значения полей 
        /// имени массива и номера UDT
        /// </summary>
        /// <param name="DB_ArrayName">Имя массива в блоке данных, SNS, SNL, DRV итд.</param>
        /// <param name="UDT_Number">Номер UDT, используется для добавления к номеру DB</param>
        public mSymbolTableItem(string DB_ArrayName, string UDT_Number)
        {
            this.DB_ArrayName = DB_ArrayName;
            this.DB_FullName = UDT_Number;
        }

        public List<string> return_ItemInOneRow()
        {
            List<string> row = new List<string>();

            row.Add(this.SignalName);
            row.Add(this.SignalAdress);
            row.Add(this.SignalDataType);
            row.Add(this.SignalComment);
            row.Add(this.SignalType);
            row.Add(this.Codename);
            row.Add(this.SystemNumber);
            row.Add(this.DeviceType);
            row.Add(this.DeviceNumber);
            row.Add(this.Etc);
            row.Add(this.DeviceTag);

            return row;
        }
    }
}
