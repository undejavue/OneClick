using System.ComponentModel;

namespace ClassLibrary.Models
{
    public class BaseEntityModel : Entity
    {

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }


        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        public BaseEntityModel()
        {

        }

        public BaseEntityModel(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public BaseEntityModel(int id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }
    }
}
