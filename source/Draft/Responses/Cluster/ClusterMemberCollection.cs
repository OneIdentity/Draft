using System;
using System.Runtime.Serialization;

namespace Draft.Responses.Cluster
{
    [DataContract]
    internal class ClusterMemberCollection
    {

        [field: IgnoreDataMember]
        private ClusterMember[] _members = Array.Empty<ClusterMember>();

        [DataMember(Name = "members")]
        public ClusterMember[] Members
        {
            get { return _members ?? (_members = new ClusterMember[0]); }
            private set { _members = value; }
        }

    }
}
