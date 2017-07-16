using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ClassLibrary.Models
{
    public class SymbolItemModel
    {

        public int ItemId { get; set; }
        public string SignalName { get; set; }
        public string SignalAdress { get; set; }
        public string SignalDataType { get; set; }
        public string SignalComment { get; set; }
        public string Codename { get; set; }
        public string SignalType { get; set; }
        public string SystemNumber { get; set; }
        public string DeviceType { get; set; }
        public string DeviceNumber { get; set; }
        public string Etc { get; set; }
        public string DeviceTag { get; set; }
        public string DbFullName { get; set; }
        public string DbArrayName { get; set; }
        public int DbArrayIndex { get; set; }


        public SymbolItemModel(SymbolTableItemModel source)
        {
            this.SignalDataType = source.SignalDataType;
            this.Codename = source.Codename;
            this.DbArrayIndex = source.DbArrayIndex;
            this.DbArrayName = source.DbArrayName;
            this.DbFullName = source.DbFullName;
            this.DeviceNumber = source.DeviceNumber;
            this.DeviceTag = source.DeviceTag;
            this.DeviceType = source.DeviceType;
            this.Etc = source.Etc;
            this.SignalAdress = source.SignalAdress;
            this.SignalComment = source.SignalComment;
            this.SignalName = source.SignalName;
            this.Codename = source.Codename;
            this.ItemId = source.ItemId;
        }

        public SymbolItemModel()
        {
           
        }

       
        public List<string> GetItemInOneRow()
        {
            var row = new List<string>();

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
