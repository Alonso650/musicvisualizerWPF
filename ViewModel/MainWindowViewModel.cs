using musicvisualizerWPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace musicvisualizerWPF.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _title;
        private double _currentTrackLength;
        private double _currentTrackPosition;
        private string _playPauseImageSource;
        private float _currentVolume;

        private AudioPlayer _audioPlayer;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string PlayPauseImage
        {
            get { return _playPauseImageSource;}
            set
            {
                if (value == _playPauseImageSource) return;
                _playPauseImageSource = value;
                OnPropertyChanged(nameof(_playPauseImageSource));
            }
        }
    }
}
