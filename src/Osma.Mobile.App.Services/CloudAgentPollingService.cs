using AgentFramework.Core.Contracts;
using Osma.Mobile.App.Services.Interfaces;
using Osma.Mobile.App.Services.Messages;
using Osma.Mobile.App.Services.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Osma.Mobile.App.Services
{
    public class CloudAgentPollingService : ICloudAgentPollingService
    {
        private CloudAgentOptions _cloudAgentOptions;

        private readonly ICustomAgentContextProvider _agentContextProvider;
        private readonly IAgentMessageProcessorService _messageProcessorService;
        private readonly IMessageService _messageService; 

        public CloudAgentPollingService(ICustomAgentContextProvider agentContextProvider, 
                                        IAgentMessageProcessorService agentMessageProcessorService, 
                                        IMessageService messageService)
        {
            _agentContextProvider = agentContextProvider;
            _messageProcessorService = agentMessageProcessorService;
            _messageService = messageService;
        }

        public async Task StartAsync(CloudAgentOptions options)
        {
            if (IsRunning)
                throw new Exception("Polling task already started");

            _cloudAgentOptions = options ?? throw new ArgumentNullException(nameof(options));

            if (options.Connection == null)
                throw new ArgumentNullException(nameof(options.Connection));

            if (options.Connection.Endpoint == null)
                throw new ArgumentException($"Endpoint of the connection is null");

            var endpointScheme = new Uri(options.Connection.Endpoint.Uri).Scheme;

            if (endpointScheme != "http" || endpointScheme != "https")
                throw new ArgumentException($"EndpointUri is invalid, scheme {endpointScheme} unknown");

            _pollTaskCancelSource = new CancellationTokenSource();

            _pollTask = StartNewPollTask(_pollTaskCancelSource);

            IsRunning = true;

            await _pollTask;
        }

        public void Stop()
        {
            if (!IsRunning)
                throw new Exception("Polling task not running");

            try
            {
                _pollTaskCancelSource.Cancel();

                IsRunning = false;
            }
            catch (Exception)
            {
                //Swallow exception
            }
        }

        private async Task PollAsync()
        {
            var context = await _agentContextProvider.GetContextAsync();

            var noopMsg = new NoopMessage();

            var msg = await _messageService.SendAsync(context.Wallet, noopMsg, _cloudAgentOptions.Connection, null, true);

            if (msg == null)
                return;

            try
            {
                await _messageProcessorService.ProcessMessageAsync(msg);
            }
            catch (Exception)
            {
                //TODO handle exception
            }
        }

        public bool IsRunning { get; private set; }

        private CancellationTokenSource _pollTaskCancelSource;
        private Task _pollTask;
        private Task StartNewPollTask(CancellationTokenSource cancellationTokenSource) => Task.Factory.StartNew(async () =>
        {
            try
            {
                while (true)
                {
                    if (cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        IsRunning = false;
                        break;
                    }

                    if (!IsRunning)
                        IsRunning = true;

                    await PollAsync();

                    var timer = Stopwatch.StartNew();

                    timer.Stop();

                    // Calculate the remaining delay to achieve the desired frequency
                    var delay = (_cloudAgentOptions.PollingIntervalSeconds * 1000) - (int)timer.ElapsedMilliseconds;

                    if (delay > 0)
                        await Task.Delay(delay, cancellationTokenSource.Token).ContinueWith(tsk => { });
                }
            }
            catch (Exception)
            { }
        }, cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }
}
