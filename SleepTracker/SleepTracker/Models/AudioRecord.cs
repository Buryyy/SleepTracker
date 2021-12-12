using Realms;
using System;

namespace SleepTracker.Models
{
    public class AudioRecord : RealmObject
    {
        [PrimaryKey] public int Id { get; set; }
        public string Path { get; set; }

        public DateTimeOffset RecordTimestamp { get; set; }
    }
}