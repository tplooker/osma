using AgentFramework.Core.Messages;
using System;

namespace Osma.Mobile.App.Services.Messages
{
    public class NoopMessage : AgentMessage
    {
        public NoopMessage()
        {
            Id = Guid.NewGuid().ToString();
            Type = "?/1.0/noop";
        }
    }
}
