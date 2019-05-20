using Acr.UserDialogs;
using AgentFramework.Core.Contracts;
using AgentFramework.Core.Models.Records;
using AgentFramework.Core.Models.Records.Search;
using Osma.Mobile.App.Services.Interfaces;
using Osma.Mobile.App.Services.Protocols.BasicMessage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Osma.Mobile.App.ViewModels.Connections.Chat
{
    public class ChatViewModel : ABaseViewModel
    {
        private ConnectionRecord _connection;
        private readonly ICustomAgentContextProvider _agentContextProvider;
        private readonly IMessageService _messageService;
        private readonly IWalletRecordService _recordService;

        public ChatViewModel(IUserDialogs userDialogs,
                             INavigationService navigationService,
                             ICustomAgentContextProvider agentContextProvider,
                             IMessageService messageService,
                             IWalletRecordService recordService) : base("Chat", userDialogs, navigationService)
        {
            _agentContextProvider = agentContextProvider;
            _messageService = messageService;
            _recordService = recordService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is ConnectionRecord connection)
            {
                _connection = connection;
                await PopulateMessagesAsync();
            }
        }

        #region Bindable Commands
        public ICommand OnSendCommand => new Command(async () =>
        {
            if (!string.IsNullOrEmpty(TextToSend))
            {
                var dialog = UserDialogs.Instance.Loading("Sending");

                string messageId = null;
                try
                {
                    messageId = await SendMessageAsync(TextToSend);
                }
                catch (Exception e)
                {

                }

                if (dialog.IsShowing)
                {
                    dialog.Hide();
                    dialog.Dispose();
                }

                if (string.IsNullOrEmpty(messageId))
                {
                    UserDialogs.Instance.Alert("Failed to send message");
                }
                else
                {
                    AddMessage(new ChatMessage()
                    {
                        Id = messageId,
                        Text = TextToSend,
                        MessageDirection = MessageDirection.Outgoing
                    });
                    TextToSend = string.Empty;
                }

            }
        });

        public ICommand MessageAppearingCommand => new Command<ChatMessage>(OnMessageAppearing);

        public ICommand MessageDisappearingCommand => new Command<ChatMessage>(OnMessageDisappearing);
        #endregion

        #region Bindable Properties
        private string _textToSend;
        public string TextToSend
        {
            get => _textToSend;
            set => this.RaiseAndSetIfChanged(ref _textToSend, value);
        }

        private bool _showScrollTap;
        public bool ShowScrollTap
        {
            get => _showScrollTap;
            set => this.RaiseAndSetIfChanged(ref _showScrollTap, value);
        }

        private bool _lastMessageVisible = true;
        public bool LastMessageVisible
        {
            get => _lastMessageVisible;
            set => this.RaiseAndSetIfChanged(ref _lastMessageVisible, value);
        }

        private int _pendingMessageCount = 0;
        public int PendingMessageCount
        {
            get => _pendingMessageCount;
            set
            {
                this.RaiseAndSetIfChanged(ref _pendingMessageCount, value);
                PendingMessageCountVisible = _pendingMessageCount > 0;
            }
        }

        private bool _pendingMessageCountVisible;
        public bool PendingMessageCountVisible
        {
            get => _pendingMessageCountVisible;
            set => this.RaiseAndSetIfChanged(ref _pendingMessageCountVisible, value);
        }

        public Queue<ChatMessage> DelayedMessages { get; set; } = new Queue<ChatMessage>();

        public ObservableCollection<ChatMessage> Messages { get; set; } = new ObservableCollection<ChatMessage>();
        #endregion

        private async Task<string> SendMessageAsync(string content)
        {
            var context = await _agentContextProvider.GetContextAsync();

            var sentTime = DateTime.UtcNow;
            var messageRecord = new BasicMessageRecord
            {
                Id = Guid.NewGuid().ToString(),
                Direction = MessageDirection.Outgoing,
                Text = content,
                SentTime = sentTime,
                ConnectionId = _connection.Id
            };
            var message = new BasicMessage
            {
                Content = content,
                SentTime = sentTime.ToString("s", CultureInfo.InvariantCulture)
            };

            // Send an agent message using the secure connection
            await _messageService.SendAsync(context.Wallet, message, _connection);

            // Save the outgoing message to the local wallet for chat history purposes
            await _recordService.AddAsync(context.Wallet, messageRecord);

            return messageRecord.Id;
        }

        private async Task PopulateMessagesAsync()
        {
            var context = await _agentContextProvider.GetContextAsync();
            var messages = await _recordService.SearchAsync<BasicMessageRecord>(context.Wallet,
                    SearchQuery.Equal(nameof(BasicMessageRecord.ConnectionId), _connection.Id), null, 100);

            foreach (var message in messages.OrderBy(_ => _.CreatedAtUtc))
            {
                AddMessage(new ChatMessage()
                {
                    Id = message.Id,
                    Text = message.Text,
                    MessageDirection = message.Direction
                });
            }
        }

        public void AddMessage(ChatMessage message)
        {
            if (LastMessageVisible)
            {
                Messages.Insert(0, message);
            }
            else
            {
                DelayedMessages.Enqueue(message);
                PendingMessageCount++;
            }
        }

        private void OnMessageAppearing(ChatMessage message)
        {
            var idx = Messages.IndexOf(message);
            if (idx <= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    while (DelayedMessages.Count > 0)
                    {
                        Messages.Insert(0, DelayedMessages.Dequeue());
                    }
                    ShowScrollTap = false;
                    LastMessageVisible = true;
                    PendingMessageCount = 0;
                });
            }
        }

        private void OnMessageDisappearing(ChatMessage message)
        {
            var idx = Messages.IndexOf(message);
            if (idx >= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ShowScrollTap = true;
                    LastMessageVisible = false;
                });

            }
        }
    }
}