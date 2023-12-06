using musicvisualizerWPF.Models;
using musicvisualizerWPF.Services;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
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

        private Track _currentlySelectedTrack;
        private Track _currentlyPlayingTrack;

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
                if (value.Equals(_currentTrackLength)) return;
                _currentTrackLength = value;
                OnPropertyChanged(nameof(_currentTrackLength));
            }
        }

        public double CurrentTrackPosition
        {
            get { return _currentTrackPosition; }
            set
            {
                if (value.Equals(_currentTrackPosition)) return;
                _currentTrackPosition = value;
                OnPropertyChanged(nameof(_currentTrackPosition));
            }
        }

        public Track CurrentlySelectedTrack
        {
            get { return _currentlySelectedTrack; }
            set
            {
                if (Equals(value, CurrentlySelectedTrack)) return;
                _currentlySelectedTrack = value;
                OnPropertyChanged(nameof(CurrentlySelectedTrack));
            }
        }

        public Track CurrentlyPlayingTrack
        {
            get { return _currentlyPlayingTrack; }
            set
            {
                if (Equals(value, _currentlyPlayingTrack)) return;
                _currentlyPlayingTrack = value;
                OnPropertyChanged(nameof(CurrentlyPlayingTrack));
            }
        }

        private enum PlaybackState
        {
            Playing, Stopped, Paused
        }

        private PlaybackState _playbackState;

        public ICommand RewindToStartCommand { get; set; }
        public ICommand StartPlaybackCommand { get; set; }
        public ICommand StopPlaybackCommand { get; set; }
        public ICommand ForwardToEndCommand { get; set; }

        public ICommand TrackControlMouseDownCommand { get; set; }
        public ICommand TrackControlMouseUpCommand { get; set; }
        public ICommand VolumeControlValueChangedCommand { get; set; }

        // Audio Player Commands
        private void RewindToStart(object p)
        {
            // Set the position to the beginning of the track
            _audioPlayer.SetPosition(0);
        }
        private bool CanRewindToStart(object p)
        {
            if (_playbackState == PlaybackState.Playing)
            {
                return true;

            }
            return false;
        }

        private void StartPlayback(object p)
        {
            if (_playbackState == PlaybackState.Stopped)
            {
                _audioPlayer = new AudioPlayer(CurrentlySelectedTrack.FilePath, CurrentVolume);

            }
        }

        private bool CanStartPlayback(object p)
        {
            return true;
        }

        private void StopPlayback(object p)
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.PlaybackStopType = AudioPlayer.PlaybackStopTypes.PlaybackStoppedByUser;
                _audioPlayer.Stop();
            }
        }

        private bool CanStopPlayBack(object p)
        {
            if (_playbackState == PlaybackState.Playing || _playbackState == PlaybackState.Paused)
            {
                return true;
            }
            return false;
        }

        private void ForwardToEnd(object p)
        {
            if(_audioPlayer != null)
            {
                _audioPlayer.PlaybackStopType = AudioPlayer.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile;
                _audioPlayer.SetPosition(_audioPlayer.GetLengthInSeconds());
            }
        }

        private bool CanForwardToEnd(object p)
        {
            if(_playbackState == PlaybackState.Playing)
            {
                return true;
            }
            return false;
        }

        // Event Commands
        private void TrackControlMouseDown(object p)
        {
            if(_audioPlayer != null)
            {
                _audioPlayer.Pause();
            } 
        }

        private bool CanTrackControlMouseDown(object p)
        {
            if(_playbackState == PlaybackState.Playing)
            {
                return true;
            }
            return false;
        }

        private void TrackControlMouseUp(object p)
        {
            if(_audioPlayer != null)
            {
                _audioPlayer.SetPosition(CurrentTrackPosition);
                _audioPlayer.Play(NAudio.Wave.PlaybackState.Paused, CurrentVolume);
            }
        }

        private bool CanTrackControlMouseUp(object p)
        {
            if(_playbackState == PlaybackState.Paused)
            {
                return true;
            }
            return false;
        }

        // Set the value of the slider to current volume
        private void VolumeControlValueChanged(object p)
        {
            if(_audioPlayer != null)
            {
                _audioPlayer.SetVolume(CurrentVolume);
            }
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
