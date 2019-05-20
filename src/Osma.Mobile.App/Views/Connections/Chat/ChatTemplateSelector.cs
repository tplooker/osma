using Osma.Mobile.App.Services.Protocols.BasicMessage;
using Osma.Mobile.App.ViewModels.Connections;
using Osma.Mobile.App.ViewModels.Connections.Chat;
using Osma.Mobile.App.Views.Connections.Chat.Cells;
using Xamarin.Forms;

namespace Osma.Mobile.App.Views.Connections.Chat
{
    public class ChatTemplateSelector : DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;

        DataTemplate outgoingDataTemplate;

        public ChatTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as ChatMessage;
            if (messageVm == null)
                return null;

            return (messageVm.MessageDirection == MessageDirection.Outgoing) ? outgoingDataTemplate : incomingDataTemplate;
        }

    }
}