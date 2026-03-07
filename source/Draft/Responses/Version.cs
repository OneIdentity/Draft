using System.Runtime.Serialization;

namespace Draft.Responses
{
    [DataContract]
    internal class Version
    {

        [DataMember(Name = "internalVersion")]
        public string InternalVersion { get; private set; } = string.Empty;

        [DataMember(Name = "releaseVersion")]
        public string ReleaseVersion { get; private set; } = string.Empty;

    }
}
