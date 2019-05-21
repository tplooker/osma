using AgentFramework.Core.Messages;
using System.Threading.Tasks;

namespace Osma.Mobile.App.Services.Interfaces
{
    public interface IAgentMessageProcessorService
    {
        Task ProcessMessageAsync(MessageContext message);
    }
}
