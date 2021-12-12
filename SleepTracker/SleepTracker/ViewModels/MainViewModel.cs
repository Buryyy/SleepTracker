using SleepTracker.Helpers;
using SleepTracker.Models;
using SleepTracker.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SleepTracker.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IAudioService _audioService;

        private bool _isPlaying;

        private RecordUIListItem _lastPlayedRecord;

        public MainViewModel(IAudioService audioService)
        {
            _audioService = audioService;
            RecordsCollection = new ObservableCollection<RecordUIListItem>();
            LoadRecords();
        }

        public void LoadRecords()
        {
            var records = _audioService.GetAllAudioRecords().ToList();
            foreach (var record in records)
            {
                RecordsCollection.Add(new RecordUIListItem
                {
                    PathOnDevice = record.Path,
                    RecordTimestamp = record.RecordTimestamp.ToString("g"),
                    RecordName = "#" + record.Path.Split('_')[1].Replace(".wav", string.Empty), //todo make this better
                    PlayIcon = "play.png"
                });
            }
        }

        public ICommand PlayAudioCommand => new Command<RecordUIListItem>(async (record) =>
         {
            //Unselect last played record.
            if (_lastPlayedRecord != null)
             {
                 await _audioService.StopPlayingRecordAsync();
                 _recordsCollection[_recordsCollection.IndexOf(_lastPlayedRecord)].PlayIcon = "play.png";
             }
             if (record != _lastPlayedRecord)
             {
                 _recordsCollection[_recordsCollection.IndexOf(record)].PlayIcon = "pause.png";
             }
             _lastPlayedRecord = record;
             await _audioService.PlayRecordingAsync(record.PathOnDevice, TimeSpan.Zero);
             Debug.WriteLine("Playing recording of " + record.PathOnDevice);
         });

        public ICommand StartRecordCommand => new Command(async () =>
         {
             bool hasRecordPermission = await PermissionHelper.CheckAndRequestMicrophoneAsync();
             if (!hasRecordPermission)
             {
                 return;
             }
             if (_isPlaying)
             {
                 _audioService.StopRecording();
                 _isPlaying = false;
             }
             else
             {
                 _audioService.StartRecording();
                 _isPlaying = true;
             }
         });

        #region UI Bindings

        private ObservableCollection<RecordUIListItem> _recordsCollection;

        public ObservableCollection<RecordUIListItem> RecordsCollection
        {
            get => _recordsCollection;
            set => SetProperty(ref _recordsCollection, value);
        }

        #endregion UI Bindings
    }
}