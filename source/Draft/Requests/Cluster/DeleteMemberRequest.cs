using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Draft.Endpoints;
using Flurl.Http;

namespace Draft.Requests.Cluster
{
    internal class DeleteMemberRequest : BaseRequest, IDeleteMemberRequest
    {

        public DeleteMemberRequest(IEtcdClient etcdClient, EndpointPool endpointPool, params string[] pathParts)
            : base(etcdClient, endpointPool, pathParts) { }

        public string MemberId { get; private set; } = string.Empty;

        public async Task Execute()
        {
            try
            {
                await TargetUrl
                    .AppendPathSegment(MemberId)
                    .DeleteAsync();
            }
            catch (FlurlHttpException e)
            {
                throw await e.ProcessException();
            }
        }

        public TaskAwaiter GetAwaiter()
        {
            return Execute().GetAwaiter();
        }

        public IDeleteMemberRequest WithMemberId(string memberId)
        {
            MemberId = memberId;
            return this;
        }

    }
}
