using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

using Draft.Configuration;
using Draft.Endpoints;
using Draft.Json;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Flurl.Http.Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[assembly: InternalsVisibleTo("Draft.Tests")]

namespace Draft
{
    /// <summary>
    ///     Factory class for configuring and creating <see cref="IEtcdClient" />s.
    /// </summary>
    public static class Etcd
    {

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private static readonly object _gate = new object();

        internal static readonly Lazy<ClientConfig> ClientConfig = new Lazy<ClientConfig>(() => new ClientConfig());

        internal static readonly JsonSerializerSettings JsonSettings;
        internal static readonly IFlurlClientCache FlurlClientCache;

        static Etcd()
        {
            JsonSettings = new JsonSerializerSettings();
            JsonSettings.Converters = JsonSettings.Converters ?? new List<JsonConverter>();
            JsonSettings.Converters.Add(new EtcdErrorCodeConverter());
            JsonSettings.Converters.Add(new StringEnumConverter { AllowIntegerValues = true });
            JsonConvert.DefaultSettings = () => JsonSettings;

            FlurlClientCache = new FlurlClientCache()
                .UseNewtonsoft(JsonSettings)
                .WithDefaults(builder =>
                {
                    builder.EventHandlers.Add((FlurlEventType.BeforeCall, new EtcdHeaderEventHandler()));
                });
        }

        public static IFlurlRequest ToRequest(this Url This)
        {
            return FlurlClientCache.GetOrAdd(This.Host, This).Request(This);
        }

        /// <summary>
        ///     <see cref="IEtcdClient" />'s global configuration options.
        /// </summary>
        public static IEtcdClientConfig Configuration => ClientConfig.Value;

        /// <summary>
        ///     Creates an <see cref="IEtcdClient" /> for the specified <see cref="EndpointPool" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">Passed <paramref name="endpointPool" /> is null.</exception>
        public static IEtcdClient ClientFor(EndpointPool endpointPool)
        {
            if (endpointPool == null)
            {
                throw new ArgumentNullException("endpointPool");
            }
            return new EtcdClient(endpointPool, ClientConfig.Value.DeepCopy());
        }

        /// <summary>
        ///     Creates an <see cref="IEtcdClient" /> for the specified <see cref="Uri" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">Passed <paramref name="uris" /> is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="uris" /> is not an absolute <see cref="Uri" />.</exception>
        public static IEtcdClient ClientFor(params Uri[] uris)
        {
            if (uris == null)
            {
                throw new ArgumentNullException("uris");
            }

            EndpointPool endpointPool = null;
            try
            {
                endpointPool = EndpointPool.Build()
                                               .WithRoutingStrategy(EndpointRoutingStrategy.RoundRobin)
                                               .WithVerificationStrategy(EndpointVerificationStrategy.None)
                                               .VerifyAndBuild(uris)
                                               .Result;
            }
            catch (AggregateException ae)
            {
                ExceptionDispatchInfo.Capture(ae.Flatten().InnerExceptions.First()).Throw();
            }


            return ClientFor(endpointPool);
        }

        /// <summary>
        ///     Provides thread-safe access to <see cref="IEtcdClient" />'s global configuration options.
        /// </summary>
        /// <remarks>
        ///     <para>Should only be called once on application initialization.</para>
        /// </remarks>
        public static void Configure(Action<IMutableEtcdClientConfig> configAction)
        {
            lock (_gate)
            {
                configAction(ClientConfig.Value);
            }
        }

        private class EtcdHeaderEventHandler : FlurlEventHandler
        {
            public override void Handle(FlurlEventType eventType, FlurlCall call)
            {
                if (call.HttpRequestMessage.IsJsonContentType())
                {
                    // This needs to be explicitly set because etcd complains if the
                    // content-type has a charset value appended
                    call.HttpRequestMessage.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(Constants.Http.ContentType_ApplicationJson);
                }
            }
        }
    }
}
