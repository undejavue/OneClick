using OneClickUI.Helpers;

namespace OneClickUI.Log
{
    public class LogTagModel : ObservableObject
    {
        private bool isSelected = true;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public LogTagModel(LogTag tag)
        {
            Id = (int) tag;
            Name = tag.ToString();
        }
    }

    public enum LogTag
    {
        All = 0,
        Debug = 1,
        Error = 2,
        Warning = 3,
        Info = 4
    }
}
