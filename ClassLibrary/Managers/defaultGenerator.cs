using System.Collections.ObjectModel;
using ClassLibrary.Models;

namespace ClassLibrary.Managers
{
    public class DefaultGenerator
    {

        public ObservableCollection<DataBlockModel> DBs
        { get; set; }


        public DefaultGenerator()
        {
            DBs = new ObservableCollection<DataBlockModel>();
            
        }

        public DataBlockModel getDBbyName(string DBsymbolName)
        {
            var resultDB = new DataBlockModel();

            foreach (var db in DBs)
            {
                if (db.Title.Equals(DBsymbolName))
                    resultDB = db;
            }


            return resultDB;
        }

    }
}
