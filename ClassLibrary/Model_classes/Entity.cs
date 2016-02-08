﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ClassLibrary
{
    public abstract class Entity : INotifyPropertyChanged
    {

        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }


        //public event CollectionChangeEventHandler CollectionChanged;

        //public void OnCollectionChanged(CollectionChangeEventArgs e)
        //{
        //    if (CollectionChanged != null)
        //        CollectionChanged(this, e);
        //}
    }
}
