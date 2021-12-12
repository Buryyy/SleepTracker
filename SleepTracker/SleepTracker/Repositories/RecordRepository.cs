using Realms;
using SleepTracker.Models;
using System.Collections.Generic;
using System.Linq;

namespace SleepTracker.Repositories
{
    public sealed class RecordRepository : IRecordRepository
    {
        private readonly Realm _realm;

        public RecordRepository(Realm realm) => _realm = realm;

        public int GetNextId()
        {
            int id = 0;
            var lastRecord = _realm.All<AudioRecord>().LastOrDefault();
            if (lastRecord != null)
            {
                id = lastRecord.Id + 1;
            }
            return id;
        }

        public void Add(AudioRecord record)
        {
            _realm.Write(() =>
            {
                _realm.Add(record);
            });
        }

        public void Remove(string path)
        {
            var record = _realm.All<AudioRecord>()
                 .FirstOrDefault(c => c.Path == path);
            _realm.Write(() =>
            {
                _realm.Remove(record);
            });
        }

        public void RemoveAll() => _realm.RemoveAll();

        public IEnumerable<AudioRecord> GetAllRecords()
        {
            return _realm.All<AudioRecord>();
        }
    }
}