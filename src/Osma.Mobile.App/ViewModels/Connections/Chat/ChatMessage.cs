using Osma.Mobile.App.Services.Protocols.BasicMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Osma.Mobile.App.ViewModels.Connections.Chat
{
    public class ChatMessage
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public MessageDirection MessageDirection { get; set; }
    }
}