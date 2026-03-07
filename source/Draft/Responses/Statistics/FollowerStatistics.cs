using System.Runtime.Serialization;

namespace Draft.Responses.Statistics
{
    /// <summary>
    ///     Various statistics about a follower in an etcd cluster
    /// </summary>
    [DataContract]
    internal class FollowerStatistics : IFollowerStatistics
    {

        IFollowerCounts IFollowerStatistics.Counts => Counts;

        /// <summary>
        ///     Follower send counts
        /// </summary>
        [DataMember(Name = "counts")]
        public FollowerCounts Counts { get; set; } = new FollowerCounts();

        IFollowerLatency IFollowerStatistics.Latency => Latency;

        /// <summary>
        ///     Follower latency statistics
        /// </summary>
        [DataMember(Name = "latency")]
        public FollowerLatency Latency { get; set; } = new FollowerLatency();

    }

}
