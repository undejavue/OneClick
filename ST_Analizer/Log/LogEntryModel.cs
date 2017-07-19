using System;
using OneClickUI.Helpers;

namespace OneClickUI.Log
{
    public class LogEntryModel : ObservableObject
    {
        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        private LogTagModel _tag;
        public LogTagModel Tag
        {
            get { return _tag; }
            set { SetProperty(ref _tag, value); }
        }


        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _time;
        public string Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }


        private string _entry;
        public string Entry
        {
            get { return _entry; }
            set { SetProperty(ref _entry, value); }
        }


        public LogEntryModel(LogTag tag, string message)
        {
            IsVisible = true;
            Tag = new LogTagModel(tag);
            Message = message;
            Time = DateTime.Now.ToString("hh:mm:ss");
            Entry = Time + ": " + Message;
        }

    }
}
