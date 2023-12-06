using musicvisualizerWPF.Models;
using musicvisualizerWPF.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

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
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string PlayPauseImage
        {
            get { return _playPauseImageSource; }
            set
            {
                if (value == _playPauseImageSource) return;
                _playPauseImageSource = value;
                OnPropertyChanged(nameof(_playPauseImageSource));
            }
        }

        public float CurrentVolume
        {
            get { return _currentVolume; }
            set
            {
                if (value.Equals(_currentVolume)) return;
                _currentVolume = value;
                OnPropertyChanged(nameof(_currentVolume));
            }
        }

        public double CurrentTrackLength
        {
            get { return _currentTrackLength; }
            set
            {
                if(value.Equals(_currentTrackLength)) return;
                _currentTrackLength = value;
                OnPropertyChanged(nameof(_currentTrackLength));
            }
        }

        public double CurrentTrackPosition
        {
            get { return _currentTrackPosition; }
            set
            {
                if(value.Equals(_currentTrackPosition)) return;
                _currentTrackPosition = value;
                OnPropertyChanged(nameof(_currentTrackPosition));
            }
        }

        public ICommand RewindToStartCommand { get; set; }
        public ICommand StartPlaybackCommand { get; set; }
        public ICommand StopPlaybackCommand { get; set; }
        public ICommand ForwardToEndCommand { get; set; }

        public ICommand TrackControlMouseDownCommand {  get; set; }
        public ICommand TrackControlMouseUpCommand { get; set; }
        public ICommand VolumeControlValueChangedCommand { get; set; }

        // Audio Player Commands
        private void RewindToStart(object p)
        {

        }
        private bool CanRewindToStart(object p)
        {
            return true;
        }

        private void StartPlayback(object p)
        {

        }

        private bool CanStartPlayback(object p)
        {
            return true;
        }

        private void StopPlayback(object p)
        {

        }

        private bool CanStopPlayBack(object p)
        {
            return true;
        }

        private void ForwardToEnd(object p)
        {

        }

        private bool CanForwardToEnd(object p)
        {
            return true;
        }

        // Event Commands
        private void TrackControlMouseDown(object p)
        {

        }

        private bool CanTrackControlMouseDown(object p)
        {
            return true;
        }

        private void TrackControlMouseUp(object p)
        {

        }

        private bool CanTrackControlMouseUp(object p)
        {
            return true;
        }

        private void VolumeControlValueChanged(object p)
        {

        }

        private bool CanVolumeControlValueChanged(object p)
        {
            return true;
        }

        private void LoadCommands()
        {
            // Audio Player Commands
            RewindToStartCommand = new RelayCommand(RewindToStart, CanRewindToStart);
            ForwardToEndCommand = new RelayCommand(ForwardToEnd, CanForwardToEnd);
            StopPlaybackCommand = new RelayCommand(StopPlayback, CanStopPlayBack);
            StartPlaybackCommand = new RelayCommand(StartPlayback, CanStartPlayback);

            // Event Commands
            TrackControlMouseDownCommand = new RelayCommand(TrackControlMouseDown, CanTrackControlMouseDown);
            TrackControlMouseUpCommand = new RelayCommand(TrackControlMouseUp, CanTrackControlMouseUp);
            VolumeControlValueChangedCommand = new RelayCommand(VolumeControlValueChanged, CanVolumeControlValueChanged);
        }

        // This is in a way like the main() where the commands
        // and other functions get called to start the application
        public MainWindowViewModel()
        {
            LoadCommands();

            // PlayList = new ObservableCollection<Track>();

            // Title = "NaudioPlayer";
            PlayPauseImageSource = "../images/PlayButton.png";
        }




    }
}
