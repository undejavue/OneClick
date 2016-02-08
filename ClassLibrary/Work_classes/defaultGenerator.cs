using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ClassLibrary.Work_classes
{
    public class defaultGenerator
    {

        public ObservableCollection<mDataBlock> DBs
        { get; set; }


        public defaultGenerator()
        {
            DBs = new ObservableCollection<mDataBlock>();
            
        }

        public mDataBlock getDBbyName(string DBsymbolName)
        {
            mDataBlock resultDB = new mDataBlock();

            foreach (mDataBlock db in DBs)
            {
                if (db.Title.Equals(DBsymbolName))
                    resultDB = db;
            }


            return resultDB;
        }

    }
}
