using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static musicvisualizerWPF.AudioPlayer;

namespace musicvisualizerWPF
{
    // provides structure for managing audio playback operations
    public class AudioPlayer
    {
        // enumeration that defines different types of playback stop events
        public enum PlaybackStopTypes
        {
            PlaybackStoppedByUser, PlaybackStoppedReachingEndOfFile
        }

        public PlaybackStopTypes PlaybackStopType { get; set; }

        private AudioFileReader _audioFileReader;

        private DirectSoundOut _output;

        private string _filepath;

        // This event is triggered when audio playback resumes. It uses
        // the 'Action' delegate, indicating that subscribers to this event
        // can attach methods that take no parameters
        public event Action PlaybackResumed;

        // Event is triggered with audio playback stops
        public event Action PlaybackStopped;

        // Event is triggered when audio playback is paused
        public event Action PlaybackPaused;

        public AudioPlayer(string filepath, float volume)
        {
            PlaybackStopType = PlaybackStopTypes.PlaybackStoppedReachingEndOfFile;

            _audioFileReader = new AudioFileReader(filepath) { Volume = volume};

            _output = new DirectSoundOut(200);
            _output.PlaybackStopped += _output_PlaybackStopped;

            var wc = new WaveChannel32(_audioFileReader);
            wc.PadWithZeroes = false;

            _output.Init(wc);

        }

        public void Play(PlaybackState playbackState, double currentVolumeLevel)
        {
            // Check if the playback state is stopped or paused.
            if(playbackState == PlaybackState.Stopped || playbackState == PlaybackState.Paused)
            {
                // If the playback state is Stopped or Paused, initiate playback
                _output.Play();
            }

            // Set the volum of the audio file reader to the specified current volume level
            _audioFileReader.Volume = (float)currentVolumeLevel;

            // Checks if the event PlaybackResumed has subscribers (event handlers)
            if(PlaybackResumed != null)
            {
                // Invoke the PlaybackResumed event (notify subscribers)
                PlaybackResumed();
            }
        }

        // If the _output object is not null then it stops the audio playback
        public void Stop()
        {
            if(_output != null)
            {
                _output.Stop();
            }
        }

        // if the _output object is not null then it pauses the audio playback
        public void Pause()
        {
            if(_output != null)
            {
                _output.Pause();

                // If there are subscribers(non-null), it invokes the event
                // notifying external components of the program that playback has paused
                if(PlaybackPaused != null)
                {
                    PlaybackPaused();
                }
            }
        }

        public void Dispose() 
        {
            if(_output != null)
            {
                // If _output is in a current playing state then stop the playback
                if (_output.PlaybackState == PlaybackState.Playing)
                {
                    _output.Stop();
                }

                // Dispose of the _output object (releasing resources)
                _output.Dispose();

                // Setting _output to null indicates it has been disposed
                _output = null;
            }

            // If _audioFileReader is not null then dispose the object
            if(_audioFileReader != null)
            {
                _audioFileReader.Dispose();
                _audioFileReader = null;
            }
        }

        public void _output_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            Dispose();
            if(PlaybackStopped != null)
            {
                PlaybackStopped();
            }
        }

        // If the _output object is not null and is playing then pause it.
        // Else if the output is not playing , start or resume playback
        // Otherwise if the object _output is null then start playback with
        // the specified volume
        public void TogglePlayPause(double currentVolumeLevel)
        {
            if(_output != null)
            {
                if(_output.PlaybackState == PlaybackState.Playing)
                {
                    Pause();
                }
                else
                {
                    Play(_output.PlaybackState, currentVolumeLevel);
                }
            }
            else
            {
                Play(PlaybackState.Stopped, currentVolumeLevel);
            }
        }

        public double GetPositionInSeconds()
        {
            return _audioFileReader != null ? _audioFileReader.CurrentTime.TotalSeconds : 0;
        }

        public double GetLengthInSeconds()
        {
            if(_audioFileReader != null)
            {
                return _audioFileReader.TotalTime.TotalSeconds;
            }
            else
            {
                return 0;
            }
        }

        public float GetVolume()
        {
            if(_audioFileReader != null)
            {
                return _audioFileReader.Volume;
            }
            return 1;
        }

        public void SetPosition(double value)
        {
            if(_audioFileReader != null)
            {
                _audioFileReader.CurrentTime = TimeSpan.FromSeconds(value);
            }
        }

        public void SetVolume(float value)
        {
            if(_output != null)
            {
                _audioFileReader.Volume = value;
            }
        }
    }

    
}
