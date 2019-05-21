using Osma.Mobile.App.Services.Models;
using System.Threading.Tasks;

namespace Osma.Mobile.App.Services.Interfaces
{
    public interface ICloudAgentPollingService
    {
        bool IsRunning { get; }

        Task StartAsync(CloudAgentOptions options);

        void Stop();
    }
}
