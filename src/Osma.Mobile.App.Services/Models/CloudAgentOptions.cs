using AgentFramework.Core.Models.Records;
using System;

namespace Osma.Mobile.App.Services.Models
{
    public class CloudAgentOptions
    {
        public ConnectionRecord Connection { get; set; }

        public int PollingIntervalSeconds { get; set; } = 5;
    }
}
