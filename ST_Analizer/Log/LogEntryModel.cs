using System;
using System.Drawing;
using System.Windows;
using OneClickUI.Helpers;

namespace OneClickUI.Log
{
    public class LogEntryModel : ObservableObject
    {
        private Visibility _isVisible;
        public Visibility IsVisible
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

        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
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

        private Color _entryColor = Color.WhiteSmoke;
        public Color EntryColor
        {
            get { return _entryColor; }
            set { SetProperty(ref _entryColor, value); }
        }


        public LogEntryModel(LogTag tag, string message, int progress = 0)
        {
            IsVisible = Visibility.Visible;
            Tag = new LogTagModel(tag);
            Message = message;
            Time = DateTime.Now.ToString("hh:mm:ss");
            Entry = Time + ": " + Message;
            Progress = 0;

            switch (tag)
            {
                case LogTag.Error:
                    EntryColor = Color.DarkRed;
                    break;
                case LogTag.Info:
                    EntryColor = Color.WhiteSmoke;
                    break;
                case LogTag.Debug:
                    EntryColor = Color.DeepSkyBlue;
                    break;
                case LogTag.Warning:
                    EntryColor = Color.OrangeRed;
                    break;
                default:
                    EntryColor = Color.WhiteSmoke;
                    break;
            }
        }

    }
}
