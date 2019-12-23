using System.Collections.Generic;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Ledger;
using Hyperledger.Indy.WalletApi;

namespace Osma.Mobile.App.Services
{
    /// <inheritdoc />
    public class AgentContext : IAgentContext
    {
        /// <inheritdoc />
        public PoolAwaitable Pool { get; set; }

        /// <inheritdoc />
        public Dictionary<string, string> State { get; set; }

        /// <inheritdoc />
        public Wallet Wallet { get; set; }
        
        /// <inheritdoc />
        public IList<MessageType> SupportedMessages { get; set; } = new List<MessageType>();

        /// <inheritdoc />
        public IAgent Agent { get; set; }
    }
}
