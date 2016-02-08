using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ClassLibrary
{
    public class mBaseEntity : Entity
    {

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }


        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        public mBaseEntity()
        {

        }

        /// <summary>
        /// Базовый класс, наследует интерфейс INotifyPropertyChanged
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="Name">Name</param>
        public mBaseEntity(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }


        /// <summary>
        /// Базовый класс, наследует интерфейс INotifyPropertyChanged
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="Name">Name</param>
        /// <param name="Description">Description</param>
        public mBaseEntity(int ID, string name, string description)
        {
            this.Id = ID;
            this.Name = name;
            this.Description = description;
        }


    }

}
