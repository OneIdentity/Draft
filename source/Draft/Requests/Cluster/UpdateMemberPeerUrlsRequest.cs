using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Draft.Endpoints;
using Draft.Responses.Cluster;
using Flurl.Http;

namespace Draft.Requests.Cluster
{
    internal class UpdateMemberPeerUrlsRequest : BaseRequest, IUpdateMemberPeerUrlsRequest
    {

        public UpdateMemberPeerUrlsRequest(IEtcdClient etcdClient, EndpointPool endpointPool, params string[] pathParts)
            : base(etcdClient, endpointPool, pathParts)
        {
            Uris = new List<Uri>();
        }

        public string MemberId { get; private set; } = string.Empty;

        public List<Uri> Uris { get; private set; }

        public async Task<IClusterMember> Execute()
        {
            var values = new FormBodyBuilder()
                .Add(Constants.Etcd.Parameter_PeerURLs, Uris.Select(x => x.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped)).ToArray())
                .Build();

            try
            {
                return await GetRequest()
                    .AppendPathSegment(MemberId)
                    .PutJsonAsync(values)
                    .ReceiveJson<ClusterMember>();
            }
            catch (FlurlHttpException e)
            {
                throw await e.ProcessException();
            }
        }

        public TaskAwaiter<IClusterMember> GetAwaiter()
        {
            return Execute().GetAwaiter();
        }

        public IUpdateMemberPeerUrlsRequest WithMemberId(string memberId)
        {
            MemberId = memberId;
            return this;
        }

        public IUpdateMemberPeerUrlsRequest WithPeerUri(params Uri[] uris)
        {
            if (uris != null && uris.Any())
            {
                Uris.AddRange(uris);
            }
            return this;
        }

    }
}
