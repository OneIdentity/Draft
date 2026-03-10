using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Draft.Endpoints;
using Draft.Responses.Statistics;
using Flurl.Http;

namespace Draft.Requests.Statistics
{
    internal class GetStoreStatisticsRequest : BaseRequest, IGetStoreStatisticsRequest
    {

        public GetStoreStatisticsRequest(IEtcdClient etcdClient, EndpointPool endpointPool, params string[] pathParts) : base(etcdClient, endpointPool, pathParts) { }

        public async Task<IStoreStatistics> Execute()
        {
            try
            {
                return await GetRequest()
                    .GetAsync()
                    .ReceiveJson<StoreStatistics>();
            }
            catch (FlurlHttpException e)
            {
                throw await e.ProcessException();
            }
        }

        public TaskAwaiter<IStoreStatistics> GetAwaiter()
        {
            return Execute().GetAwaiter();
        }

    }
}
