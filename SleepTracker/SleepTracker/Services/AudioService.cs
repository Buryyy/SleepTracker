using MediaManager;
using Plugin.AudioRecorder;
using SleepTracker.Models;
using SleepTracker.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SleepTracker.Services
{
    /// <summary>
    /// TODO: A-lot of re-writing here :)
    /// </summary>
    public class AudioService : IAudioService
    {
        private readonly IRecordRepository _recordRepository;

        private AudioRecorderService _recorder;

        private Task _recordTask;

        public AudioService(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public void StartRecording()
        {
            _recorder ??= new AudioRecorderService //needs a new instance to record
            {
                FilePath = Path.Combine(Path.GetTempPath(), $"Record_{_recordRepository.GetNextId()}"),
                StopRecordingOnSilence = true, //will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = false, //stop recording after a max timeout (defined below)
                TotalAudioTimeout = TimeSpan.FromSeconds(180) //audio will stop recording after 3 minutes
            };
            _recordTask = Task.Run(async () => await RecordAudioAsync());
        }

        public IEnumerable<AudioRecord> GetAllAudioRecords()
        {
            return _recordRepository.GetAllRecords();
        }

        public void StopRecording()
        {
            if (_recordTask != null)
            {
                _recorder.StopRecording();

                //Non-null indicating the saving of this audio was successful.
                if (_recorder.FilePath != null)

                {
                    bool didRetrieveId = int.TryParse(_recorder.FilePath.Split('_')[1]
                        .Replace(".wav", string.Empty), out var recordId);  //Format being e.g: *droid_path*/Record_1.wav

                    if (didRetrieveId)
                    {
                        _recordRepository.Add(new AudioRecord
                        {
                            Id = recordId,
                            Path = _recorder.FilePath,
                            RecordTimestamp = DateTime.Now,
                        });
                    }
                }
            }
        }

        public async Task PlayRecordingAsync(string path, TimeSpan position)
        {
            await CrossMediaManager.Current.Stop();
            await CrossMediaManager.Current.SeekTo(position);
            await CrossMediaManager.Current.Play("file://" + path);
        }

        public async Task StopPlayingRecordAsync()
        {
            if (CrossMediaManager.Current.IsPlaying())
                await CrossMediaManager.Current.Stop();
        }

        private async Task RecordAudioAsync()
        {
            try
            {
                if (!_recorder.IsRecording)
                    await _recorder.StartRecording();
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e.StackTrace);
#else
                Analytics.TrackEvent("RecordAudioAsync", new Dictionary<string, string>
                {
                    {  "Exception", e.Message },
                    {"StackTrace", e.StackTrace },
                    {"_recorder.IsRecording", _recorder.IsRecording.ToString() } }
                );
#endif
            }

        }
    }
}
