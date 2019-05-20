using AgentFramework.Core.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Osma.Mobile.App.Services.Protocols.BasicMessage
{
    public class BasicMessage : AgentMessage
    {
        public BasicMessage()
        {
            Id = Guid.NewGuid().ToString();
            Type = "did:sov:BzCbsNYhMrjHiqZDTUASHg;spec/basicmessage/1.0/message";
        }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("sent_time")]
        public string SentTime { get; set; }
    }
}