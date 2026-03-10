using System.Runtime.Serialization;

namespace Draft.Responses
{
    [DataContract]
    internal class EtcdVersion : IEtcdVersion
    {

        [DataMember(Name = "releaseVersion")]
        public string Internal { get; private set; } = string.Empty;

        [DataMember(Name = "internalVersion")]
        public string Release { get; private set; } = string.Empty;

    }
}
