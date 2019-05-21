using AgentFramework.Core.Contracts;
using AgentFramework.Core.Messages;
using AgentFramework.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Osma.Mobile.App.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Osma.Mobile.App.Services
{
    public class AgentMessageProcessorService : AgentMessageProcessorBase, IAgentMessageProcessorService
    {
        private IAgentContext _agentContext;
        private readonly IAgentContextProvider _agentContextProvider;

        public AgentMessageProcessorService(IServiceProvider provider) : base(provider)
        {
            _agentContextProvider = provider.GetService<IAgentContextProvider>();
            _initializeTask = InitializeAsync();
        }

        private Task _initializeTask;
        public async Task InitializeAsync()
        {
            _agentContext = await _agentContextProvider.GetContextAsync();
        }

        public Task ProcessMessageAsync(MessageContext msg) => ProcessAsync(_agentContext, msg);

        protected override void ConfigureHandlers()
        {
            AddConnectionHandler();
            AddCredentialHandler();
            AddProofHandler();
            AddTrustPingHandler();

            base.ConfigureHandlers();
        }
    }
}
