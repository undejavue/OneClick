using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    public partial class mDataBlock: mBaseEntity
    {
        //[ForeignKey("mCategory")]
        public int CategoryId { get; set; }
        
        
        /// <summary>
        /// Название блока данных
        /// </summary>
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }

        /// <summary>
        /// Полное имя блока данных: DB101
        /// </summary>
        private string _FullName;
        public string FullName
        {
            get { return _FullName; }
            set
            {
                _FullName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FullName"));
            }
        }

        /// <summary>
        /// Символьное имя блока данных, '10A'
        /// </summary>
        private string _SymbolName;
        public string SymbolName
        {
            get { return _SymbolName; }
            set
            {
                _SymbolName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SymbolName"));
            }
        }

        /// <summary>
        /// Номер блока данных = номер системы, '10'
        /// </summary>
        private string _Number;
        public string Number
        {
            get { return _Number; }
            set
            {
                _Number = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Number"));
            }
        }

        /// <summary>
        /// Символ дата блока = тип устройства, 'A'
        /// </summary>
        private string _Symbol;
        public string Symbol
        {
            get { return _Symbol; }
            set
            {
                _Symbol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Symbol"));
            }
        }

        /// <summary>
        /// Имя массива 'SNS'
        /// </summary>
        private string _ArrayName;
        public string ArrayName
        {
            get { return _ArrayName; }
            set
            {
                _ArrayName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArrayName"));
            }
        }

        /// <summary>
        /// Имя пользовательского блока UDT, 'SNS_UDT'
        /// </summary>
        private string _UDT_Name;
        public string UDT_Name
        {
            get { return _UDT_Name; }
            set
            {
                _UDT_Name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UDT_Name"));
            }
        }

        /// <summary>
        /// Номер UDT, '1', используется для форм. полного имени DB
        /// </summary>
        private string _UDT_Number;
        public string UDT_Number
        {
            get { return _UDT_Number; }
            set
            {
                _UDT_Number = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UDT_Number"));
            }
        }

        /// <summary>
        /// Для формирования списка блоков данных в это поле записывается максимальный индекс!
        /// S7 индекс в массиве,  SNS[i]
        /// </summary>
        private int _ArrayIndex;
        public int MaxArrayIndex
        {
            get { return _ArrayIndex; }
            set
            {
                _ArrayIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArrayIndex"));
            }
        }




        /// <summary>
        /// Link to model mCategory
        /// </summary>
        public virtual mCategory mCategory { get; set; }

        //public mDataBlock() { }
    }
}
