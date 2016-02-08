using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ClassLibrary;

namespace ClassLibrary
{
    public partial class mSymbolTableItem: mBaseEntity
    {

        public int itemId { get; set; }
        
        /// <summary>
        /// Symbol Name, like Q_22M03_ON_ETC
        /// </summary>
        private string _SignalName;
        public string SignalName
        {
            get { return _SignalName; }
            set
            {
                _SignalName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SymbolName"));
            }
        }

        /// <summary>
        /// S7 adress, like IW120, Q0.2 etc...
        /// </summary>
        private string _SignalAdress;
        public string SignalAdress
        {
            get { return _SignalAdress; }
            set
            {
                _SignalAdress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SignalAdress"));
            }
        }

        /// <summary>
        /// s7 type: BOOL, WORD etc...
        /// </summary>
        private string _SignalDataType;
        public string SignalDataType
        {
            get { return _SignalDataType; }
            set
            {
                _SignalDataType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SignalDataType"));
            }
        }

        /// <summary>
        /// SignalComment: full signal description, comment in symbol table
        /// </summary>
        private string _SignalComment;
        public string SignalComment
        {
            get { return _SignalComment; }
            set
            {
                _SignalComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SignalComment"));
            }
        }
        private string _Codename;
        public string Codename
        {
            get { return _Codename; }
            set
            {
                _Codename = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Codename"));
            }
        }

        /// <summary>
        /// SignalType mean IW, QW, I, Q
        /// </summary>
        private string _SignalType;
        public string SignalType
        {
            get { return _SignalType; }
            set
            {
                _SignalType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SignalType"));
            }
        }

        /// <summary>
        /// SystemNumber in 25M03_ON is '25'
        /// </summary>
        private string _SystemNumber;
        public string SystemNumber
        {
            get { return _SystemNumber; }
            set
            {
                _SystemNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SystemNumber"));
            }
        }

        /// <summary>
        /// DeviceType in 25M03_ON is 'M'
        /// </summary>
        private string _DeviceType;
        public string DeviceType
        {
            get { return _DeviceType; }
            set
            {
                _DeviceType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeviceType"));
            }
        }

        /// <summary>
        /// DeviceNumber in 25M03_ON is 03'
        /// </summary>
        private string _DeviceNumber;
        public string DeviceNumber
        {
            get { return _DeviceNumber; }
            set
            {
                _DeviceNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeviceNumber"));
            }
        }

        /// <summary>
        /// Etc in 25M03_ON is ON'
        /// </summary>
        private string _Etc;
        public string Etc
        {
            get { return _Etc; }
            set
            {
                _Etc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Etc"));
            }
        }

        /// <summary>
        /// DeviceTag is 'Ctr' for 'ON' etc..
        /// </summary>
        private string _DeviceTag;
        public string DeviceTag
        {
            get { return _DeviceTag; }
            set
            {
                _DeviceTag = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeviceTag"));
            }
        }


        /// <summary>
        /// Имя блока данных, 'DB101'
        /// </summary>
        private string _DB_FullName;
        public string DB_FullName
        {
            get { return _DB_FullName; }
            set
            {
                _DB_FullName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DB_FullName"));
            }
        }

        /// <summary>
        /// Имя массива в блоке данных, 'SNS'
        /// </summary>
        private string _DB_ArrayName;
        public string DB_ArrayName
        {
            get { return _DB_ArrayName; }
            set
            {
                _DB_ArrayName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DB_ArrayName"));
            }
        }

        private int _DB_ArrayIndex;
        public int DB_ArrayIndex
        {
            get { return _DB_ArrayIndex; }
            set
            {
                _DB_ArrayIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DB_ArrayIndex"));
            }
        }

        /// <summary>
        /// Text of STL source code for signal
        /// </summary>
        private ObservableCollection<mBaseEntity> _peripheryCode;
        public virtual ObservableCollection <mBaseEntity> peripheryCode
        {
            get { return _peripheryCode; }
            set
            {
                _peripheryCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("peripheryCode"));
            }
        }



        public virtual mCategory mCategory { get; set; }


        public mSymbolTableItem()
        {
            peripheryCode = new ObservableCollection <mBaseEntity>();
        }


    }
}
