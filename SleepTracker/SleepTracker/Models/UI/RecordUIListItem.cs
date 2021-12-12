using System.ComponentModel;

namespace SleepTracker.Models
{
    public class RecordUIListItem : INotifyPropertyChanged
    {
        public string RecordName { get; set; }
        public string PlayIcon { get; set; }

        public string PathOnDevice { get; set; }
        public string RecordTimestamp { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}