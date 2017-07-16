using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ClassLibrary.Models
{
    public class SymbolTableItemModel: BaseEntityModel
    {

        public int ItemId { get; set; }
        
        /// <summary>
        /// Symbol Name, like Q_22M03_ON_ETC
        /// </summary>
        private string _signalName;
        public string SignalName
        {
            get { return _signalName; }
            set
            {
                _signalName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SymbolName"));
            }
        }

        /// <summary>
        /// S7 adress, like IW120, Q0.2 etc...
        /// </summary>
        private string _signalAdress;
        public string SignalAdress
        {
            get { return _signalAdress; }
            set
            {
                _signalAdress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SignalAdress"));
            }
        }

        /// <summary>
        /// s7 type: BOOL, WORD etc...
        /// </summary>
        private string _signalDataType;
        public string SignalDataType
        {
            get { return _signalDataType; }
            set
            {
                _signalDataType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SignalDataType"));
            }
        }

        /// <summary>
        /// SignalComment: full signal description, comment in symbol table
        /// </summary>
        private string _signalComment;
        public string SignalComment
        {
            get { return _signalComment; }
            set
            {
                _signalComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SignalComment"));
            }
        }
        private string _codename;
        public string Codename
        {
            get { return _codename; }
            set
            {
                _codename = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Codename"));
            }
        }

        /// <summary>
        /// SignalType mean IW, QW, I, Q
        /// </summary>
        private string _signalType;
        public string SignalType
        {
            get { return _signalType; }
            set
            {
                _signalType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SignalType"));
            }
        }

        /// <summary>
        /// SystemNumber in 25M03_ON is '25'
        /// </summary>
        private string _systemNumber;
        public string SystemNumber
        {
            get { return _systemNumber; }
            set
            {
                _systemNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SystemNumber"));
            }
        }

        /// <summary>
        /// DeviceType in 25M03_ON is 'M'
        /// </summary>
        private string _deviceType;
        public string DeviceType
        {
            get { return _deviceType; }
            set
            {
                _deviceType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeviceType"));
            }
        }

        /// <summary>
        /// DeviceNumber in 25M03_ON is 03'
        /// </summary>
        private string _deviceNumber;
        public string DeviceNumber
        {
            get { return _deviceNumber; }
            set
            {
                _deviceNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeviceNumber"));
            }
        }

        /// <summary>
        /// Etc in 25M03_ON is ON'
        /// </summary>
        private string _etc;
        public string Etc
        {
            get { return _etc; }
            set
            {
                _etc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Etc"));
            }
        }

        /// <summary>
        /// DeviceTag is 'Ctr' for 'ON' etc..
        /// </summary>
        private string _deviceTag;
        public string DeviceTag
        {
            get { return _deviceTag; }
            set
            {
                _deviceTag = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeviceTag"));
            }
        }


        /// <summary>
        /// Имя блока данных, 'DB101'
        /// </summary>
        private string _dbFullName;
        public string DbFullName
        {
            get { return _dbFullName; }
            set
            {
                _dbFullName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DbFullName"));
            }
        }

        /// <summary>
        /// Имя массива в блоке данных, 'SNS'
        /// </summary>
        private string _dbArrayName;
        public string DbArrayName
        {
            get { return _dbArrayName; }
            set
            {
                _dbArrayName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DbArrayName"));
            }
        }

        private int _dbArrayIndex;
        public int DbArrayIndex
        {
            get { return _dbArrayIndex; }
            set
            {
                _dbArrayIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DbArrayIndex"));
            }
        }

        /// <summary>
        /// Text of STL source code for signal
        /// </summary>
        public ObservableCollection <BaseEntityModel> PeripheryCode { get; set; }

        public virtual CategoryModel CategoryModel { get; set; }

        public SymbolTableItemModel()
        {
            PeripheryCode = new ObservableCollection<BaseEntityModel>();
            CategoryModel = new CategoryModel();
        }

        /// Для каждого созданного элемента сразу задаются значения полей 
        /// имени массива и номера UDT
        /// </summary>
        /// <param name="dbArrayName">Имя массива в блоке данных, SNS, SNL, DRV итд.</param>
        /// <param name="udtNumber">Номер UDT, используется для добавления к номеру DB</param>
        public SymbolTableItemModel(string dbArrayName, string udtNumber)
        {
            this.DbArrayName = dbArrayName;
            this.DbFullName = udtNumber;
            PeripheryCode = new ObservableCollection<BaseEntityModel>();
            CategoryModel = new CategoryModel();

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
