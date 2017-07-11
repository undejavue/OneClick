using System;
using System.ComponentModel;
using ClassLibrary.Models;

namespace OneClickUI
{
    public class Globals : Entity
    {
        private string _filename;
        public string filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
                OnPropertyChanged(new PropertyChangedEventArgs("filename"));
            }

        }

        private string _DBfilename;
        public string DBfilename
        {
            get { return _DBfilename; }
            set
            {
                _DBfilename = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DBfilename"));
            }

        }
        private bool _isExcelVisible;
        public bool isExcelVisible
        {
            get { return _isExcelVisible; }
            set
            {
                _isExcelVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("isExcelVisible"));
            }

        }


        private int _categoriesCount;
        public int categoriesCount
        {
            get { return _categoriesCount; }
            set
            {
                _categoriesCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("categoriesCount"));
            }
        }


        private string _rootdir;
        public string rootdir
        {
            get { return _rootdir; }
            set
            {
                _rootdir = value;
                sourcedir = _rootdir + "\\sources";
                staticRootDir = value;

                OnPropertyChanged(new PropertyChangedEventArgs("rootdir"));
                OnPropertyChanged(new PropertyChangedEventArgs("sourcedir"));
            }
        }

        private static string staticRootDir { get; set; }

        public string sourcedir { get; set; }

        public Globals()
        {
            isExcelVisible = true;
            filename = "путь не задан";
            DBfilename = "путь не задан";
            categoriesCount = 0;

            rootdir = AppDomain.CurrentDomain.BaseDirectory;
        }


        public static string get_RootDir()
        {

            return staticRootDir;
        }

    }
}
