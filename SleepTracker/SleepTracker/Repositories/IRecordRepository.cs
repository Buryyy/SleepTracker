using SleepTracker.Models;
using System.Collections.Generic;

namespace SleepTracker.Repositories
{
    public interface IRecordRepository
    {
        void Add(AudioRecord record);

        int GetNextId();

        void Remove(string path);

        void RemoveAll();

        IEnumerable<AudioRecord> GetAllRecords();
    }
}