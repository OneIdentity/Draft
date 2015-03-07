﻿using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Draft.Responses.Statistics
{
    /// <summary>
    ///     Various statistics about a follower in an etcd cluster
    /// </summary>
    [DataContract]
    internal class FollowerStatistics
    {

        /// <summary>
        ///     Follower send counts
        /// </summary>
        [DataMember(Name = "counts")]
        public FollowerCounts Counts { get; private set; }

        /// <summary>
        ///     Follower latency statistics
        /// </summary>
        [DataMember(Name = "latency")]
        public FollowerLatency Latency { get; private set; }

    }
}
