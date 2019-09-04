using Prism.Mvvm;

namespace Tracker.Core.Models
{
    public class Staff : BindableBase
    {
        private int _id;
        private string _name;
        private bool _isActive;
        private bool _isChanged = false;
        
        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                IsChanged = true;
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                SetProperty(ref _isActive, value);
                IsChanged = true;
            }
        }

        public bool IsChanged
        {
            get { return _isChanged; }
            set { SetProperty(ref _isChanged, value); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
