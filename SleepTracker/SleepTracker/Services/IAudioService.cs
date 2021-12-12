using SleepTracker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SleepTracker.Services
{
    public interface IAudioService
    {
        IEnumerable<AudioRecord> GetAllAudioRecords();

        Task PlayRecordingAsync(string path, TimeSpan position);

        void StartRecording();

        Task StopPlayingRecordAsync();

        void StopRecording();
    }
}