using Draft.Endpoints;

using Flurl;
using Flurl.Http;
using System;

namespace Draft.Requests
{
    internal abstract class BaseRequest
    {

        private readonly string[] _pathParts;

        private readonly EndpointPool _endpointPool;

        protected BaseRequest(IEtcdClient etcdClient, EndpointPool endpointPool, params string[] pathParts)
        {
            EtcdClient = etcdClient;
            _endpointPool = endpointPool;
            _pathParts = pathParts;
        }

        public IEtcdClient EtcdClient { get; private set; }

        public Url TargetUrl => _endpointPool.GetEndpointUrl(_pathParts);

        protected TimeSpan? EndpointPoolHttpTimeout => _endpointPool.HttpGetTimeout;

        public IFlurlRequest GetRequest()
        {
            return TargetUrl.ToClient().Request();
        }
    }
}
