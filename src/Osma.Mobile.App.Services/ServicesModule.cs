using System.Net.Http;
using Autofac;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.Discovery;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using Hyperledger.Aries.Ledger;
using Hyperledger.Aries.Payments;
using Hyperledger.Aries.Runtime;
using Hyperledger.Aries.Storage;

namespace Osma.Mobile.App.Services
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterType<HttpMessageDispatcher>()
                .As<IMessageDispatcher>();

            builder
                .RegisterType<HttpClientHandler>()
                .As<HttpMessageHandler>();

            builder
                .RegisterType<EventAggregator>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<AgentContextProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultWalletRecordService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultWalletService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultPoolService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultConnectionService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultCredentialService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultProofService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultProvisioningService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultMessageService>()
                .AsImplementedInterfaces()
                .SingleInstance();
            
            builder
                .RegisterType<DefaultLedgerService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DefaultSchemaService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DefaultTailsService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DefaultDiscoveryService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DefaultPaymentService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DefaultLedgerSigningService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DefaultHttpClientFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
