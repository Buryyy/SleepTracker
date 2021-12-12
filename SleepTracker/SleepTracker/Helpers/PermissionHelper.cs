using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SleepTracker.Helpers
{
    public static class PermissionHelper
    {
        public static async Task<bool> CheckAndRequestMicrophoneAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            if (status != PermissionStatus.Granted)
            {
                var request = await Permissions.RequestAsync<Permissions.Microphone>();
                if (request != PermissionStatus.Granted)
                {
                    return false;
                }
            }
            return true;
        }
    }
}