using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace musicvisualizerWPF
{
    public class Track : INotifyPropertyChanged
    {
        private string _name;
        private string _friendlyName;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string FriendlyName
        {
            get { return _friendlyName; }
            set
            {
                if(value == _friendlyName) return;
                _friendlyName = value;
                OnPropertyChanged(nameof(FriendlyName));

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Track(string name, string friendlyName)
        {
            Name = name;
            FriendlyName = friendlyName;
        }
    }
}
