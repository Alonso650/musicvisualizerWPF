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
using System.Windows;
using System.Collections.ObjectModel;
using Microsoft.Win32;

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
        private ObservableCollection<Track> _playlist;
        private AudioPlayer _audioPlayer;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string PlayPauseImageSource
        {
            get { return _playPauseImageSource; }
            set
            {
                if (value == _playPauseImageSource) return;
                _playPauseImageSource = value;
                OnPropertyChanged(nameof(PlayPauseImageSource));
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

        public ObservableCollection<Track> Playlist
        {
            get { return _playlist; }
            set
            {
                if (Equals(value, _playlist)) return;
                _playlist = value;
                OnPropertyChanged(nameof(Playlist));
            }
        }

        private enum PlaybackState
        {
            Playing, Stopped, Paused
        }

        private PlaybackState _playbackState;
        public ICommand ExitApplicationCommand { get; set; }
        // not sure about this one yet since im trying to test one file
        // and not a full playlist
        public ICommand AddFileToPlaylistCommand { get; set; }
        public ICommand LoadPlaylistCommand { get; set; }

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
            if(CurrentlySelectedTrack != null)
            {
                if (_playbackState == PlaybackState.Stopped)
                {
                    // error says it doesnt exist and check with the AudioPlayer class file
                    _audioPlayer = new AudioPlayer(CurrentlySelectedTrack.Name, CurrentVolume);
                    _audioPlayer.PlaybackStopType = AudioPlayer.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile;
                    _audioPlayer.PlaybackPaused += _audioPlayer_PlaybackPaused;
                    _audioPlayer.PlaybackResumed += _audioPlayer_PlaybackResumed;
                    _audioPlayer.PlaybackStopped += _audioPlayer_PlaybackStopped;
                    CurrentTrackLength = _audioPlayer.GetLengthInSeconds();
                    CurrentlyPlayingTrack = CurrentlySelectedTrack;
                }
                if(CurrentlySelectedTrack == CurrentlyPlayingTrack)
                {
                    _audioPlayer.TogglePlayPause(CurrentVolume);
                }
            }
            
        }


        private bool CanStartPlayback(object p)
        {
            if(CurrentlySelectedTrack != null)
            {
                return true;
            }
            return false;
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

        // Menu Commands
        private void AddFileToPlaylist(object p)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Audio files (*.wav, *.wma, *.mp3) | *.wav; *.mp3; *.wma;";
            var result = ofd.ShowDialog();
            if(result == true)
            {
                var friendlyName = ofd.SafeFileName.Remove(ofd.SafeFileName.Length - 4);
                var track = new Track(ofd.FileName, friendlyName);
                Playlist.Add(track);
            }

        }

        private bool CanAddFileToPlaylist(object p)
        {
            if(_playbackState == PlaybackState.Stopped)
            {
                return true;
            }
            return false;
            
        }

        private void ExitApplication(object p)
        {
            if(_audioPlayer != null)
            {
                _audioPlayer.Dispose();
            }

            Application.Current.Shutdown();
        }

        private bool CanExitApplication(object p)
        {
            return true;
        }

        private void LoadPlaylist(object p)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Playlist files (*.playlist) | *.playlist";
            /*
            if(ofd.ShowDialog() == true)
            {
                Playlist = new PlaylistLoader().Load(ofd.FileName).ToObservableCollection(); // load the playlist
            }
            */
            if(ofd.ShowDialog() == true)
            {
                var playlistLoader = new PlaylistLoader();
                List<Track> loadedTracks = playlistLoader.Load(ofd.FileName);

                // Convert the List<Track> to an ObservableColelction<Track>
                Playlist = new ObservableCollection<Track>(loadedTracks);
            }
        }

        private bool CanLoadPlaylist(object p)
        {
            return true; 
        }

        private void LoadCommands()
        {
            // Menu commands
            ExitApplicationCommand = new RelayCommand(ExitApplication, CanExitApplication);
            LoadPlaylistCommand = new RelayCommand(LoadPlaylist, CanLoadPlaylist);
            AddFileToPlaylistCommand = new RelayCommand(AddFileToPlaylist, CanAddFileToPlaylist);

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

        // code (not sure if using it) to go on the next song in a playlist
        /*
        public static T NextItem<T>(this ObservableCollection<T> collection, T currentItem)
        {
            var currentIndex = collection.IndexOf(currentItem);
            if(currentIndex < collection.Count - 1)
            {
                return collection[currentIndex + 1];
            }
            return collection[0];
        }
        */

        private void _audioPlayer_PlaybackStopped()
        {
            _playbackState = PlaybackState.Stopped;
            PlayPauseImageSource = "../images/PlayButton.png";
            CommandManager.InvalidateRequerySuggested();
            CurrentTrackPosition = 0;

            if (_audioPlayer.PlaybackStopType == AudioPlayer.PlaybackStopTypes.PlaybackStoppedReachingEndOfFile)
            {
                CurrentlySelectedTrack = Playlist.NextItem(CurrentlyPlayingTrack);
                StartPlayback(null);
            }
        }

        // These two methods with the audioplayer will
        // change the image of button from pause to play and
        // vice versa 
        private void _audioPlayer_PlaybackResumed()
        {
            _playbackState = PlaybackState.Playing;
            PlayPauseImageSource = "../images/PauseButton.png";
        }

        private void _audioPlayer_PlaybackPaused()
        {
            _playbackState = PlaybackState.Paused;
            _playPauseImageSource = "../images/PlayButton.png";

        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if(_audioPlayer != null)
            {
                _audioPlayer.Dispose();
            }
        }

        // This is in a way like the main() where the commands
        // and other functions get called to start the application
        public MainWindowViewModel()
        {
            Application.Current.MainWindow.Closing += MainWindow_Closing;

            Title = "Media Player";

            LoadCommands();

            Playlist = new ObservableCollection<Track>();

            _playbackState = PlaybackState.Stopped;

            PlayPauseImageSource = "../images/PlayButton.png";
            CurrentVolume = 1;
            

        }

    }

    // Separation for Reusability
    public static class ObservableCollectionExtensions
    {
        public static T NextItem<T>(this ObservableCollection<T> collection, T currentItem)
        {
            var currentIndex = collection.IndexOf(currentItem);
            if(currentIndex < collection.Count - 1)
            {
                return collection[currentIndex + 1];
            }
            return collection[0];
        }
    }
}
