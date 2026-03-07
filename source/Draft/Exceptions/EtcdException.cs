using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;

using Draft.Responses;

using Newtonsoft.Json;

namespace Draft.Exceptions
{
    /// <summary>
    ///     Base exception that is thrown when etcd returns an error response.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public abstract class EtcdException : Exception
    {
        /// <summary>
        ///     Initializes a new <see cref="EtcdException" /> instance.
        /// </summary>
        protected EtcdException()
        {
        }

        /// <summary>
        ///     Initializes a new <see cref="EtcdException" /> instance with a specified error message.
        /// </summary>
        protected EtcdException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new <see cref="EtcdException" /> instance for use in BCL deserialization
        /// </summary>
        protected EtcdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            info.TryGetString(nameof(EtcdError), x => EtcdError = JsonConvert.DeserializeObject<EtcdError>(x) ?? new EtcdError());
            info.TryGetString(nameof(EtcdError), x => HttpStatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), x, true));
            info.TryGetString(nameof(RequestMethod), x => RequestMethod = new HttpMethod(x));
            info.TryGetString(nameof(RequestUrl), x => RequestUrl = x);
        }

        /// <summary>
        ///     The parsed etcd error message if available.
        /// </summary>
        public IEtcdError EtcdError { get; internal set; } = new EtcdError();

        /// <summary>
        ///     The <see cref="System.Net.HttpStatusCode" /> if the operation returned a response.
        /// </summary>
        public HttpStatusCode? HttpStatusCode { get; internal set; }

        /// <summary>
        ///     Indicates that this exception is due to a general problem with the request to etcd.
        /// </summary>
        public virtual bool IsBadRequest => false;

        /// <summary>
        ///     Indicates that this exception is due to an internal client error
        /// </summary>
        public virtual bool IsClientInternal => false;

        /// <summary>
        ///     Indicates that this exception is due to the passed directory still containing children.
        /// </summary>
        public virtual bool IsDirectoryNotEmpty => false;

        /// <summary>
        ///     Indicates that this exception is due to the event in the requested index is outdated and cleared.
        /// </summary>
        public virtual bool IsEventIndexCleared => false;

        /// <summary>
        ///     Indicates that this exception is due to there being an existing peer address that matches the value passed.
        /// </summary>
        public virtual bool IsExistingPeerAddress => false;

        /// <summary>
        ///     Indicates that this exception is due to an underlying http client connection error.
        /// </summary>
        public virtual bool IsHttpConnection => false;

        /// <summary>
        ///     Indicates that this exception is due to etcd being unable to parse the passed index value as a number.
        /// </summary>
        public virtual bool IsIndexNotANumber => false;

        /// <summary>
        ///     Indicates that this exception is due to the request missing the index or value property.
        /// </summary>
        public virtual bool IsIndexOrValueRequired => false;

        /// <summary>
        ///     Indicates that this exception is due to "Index and value cannot both be specified."
        /// </summary>
        public virtual bool IsIndexValueMutex => false;

        /// <summary>
        ///     Indicates that this exception is due to an "Invalid active size" error.
        /// </summary>
        public virtual bool IsInvalidActiveSize => false;

        /// <summary>
        ///     Indicates that this exception is due to an invalid field value.
        /// </summary>
        public virtual bool IsInvalidField => false;

        /// <summary>
        ///     Indicates that this exception is due to an invalid form post.
        /// </summary>
        public virtual bool IsInvalidForm => false;

        /// <summary>
        ///     Indicates that this exception is due to attempting to connect to a non-etcd endpoint.
        /// </summary>
        public virtual bool IsInvalidHost => false;

        /// <summary>
        ///     Indicates that this exception is due to a "Standby remove delay" error.
        /// </summary>
        public virtual bool IsInvalidRemoveDelay => false;

        /// <summary>
        ///     Indicates that this exception is due to an invalid request error.
        /// </summary>
        public virtual bool IsInvalidRequest => false;

        /// <summary>
        ///     Indicates that this exception is due to attempting to use an etcd reserved keyword as a key operation key.
        /// </summary>
        public virtual bool IsKeyIsPreserved => false;

        /// <summary>
        ///     Indicates that this exception is due to the passed keyspace key not existing.
        /// </summary>
        public virtual bool IsKeyNotFound => false;

        /// <summary>
        ///     Indicates that this exception is due to an in process leader election.
        /// </summary>
        public virtual bool IsLeaderElect => false;

        /// <summary>
        ///     Indicates that this exception is due to the name field is missing in the form post.
        /// </summary>
        public virtual bool IsNameRequired => false;

        /// <summary>
        ///     Indicates that this exception is due to the passed key already existing.
        /// </summary>
        public virtual bool IsNodeExists => false;

        /// <summary>
        ///     Indicates that this exception is due to reaching the max number of peers in the cluster.
        /// </summary>
        public virtual bool IsNoMorePeer => false;

        /// <summary>
        ///     Indicates that this exception is due to attempting a directory based operation on a key that isn't a directory.
        /// </summary>
        public virtual bool IsNotDirectory => false;

        /// <summary>
        ///     Indicates that this exception is due to attempting a file based operation on a key that isn't a file.
        /// </summary>
        public virtual bool IsNotFile => false;

        /// <summary>
        ///     Indicates that this exception is due to the previous value field missing in the form post.
        /// </summary>
        public virtual bool IsPreviousValueRequired => false;

        /// <summary>
        ///     Indicates that this exception is due to an internal raft error.
        /// </summary>
        public virtual bool IsRaftInternal => false;

        /// <summary>
        ///     Indicates that this exception is due to the root keyspace being read only.
        /// </summary>
        /// <remarks>You probably tried to set a value on the root keyspace.</remarks>
        public virtual bool IsRootReadOnly => false;

        /// <summary>
        ///     Indicates that this exception is due to an "Service unavailable error".
        /// </summary>
        public virtual bool IsServiceUnavailable => false;

        /// <summary>
        ///     Indicates that this exception is due to an "Internal standby error".
        /// </summary>
        public virtual bool IsStandbyInternal => false;

        /// <summary>
        ///     Indicates that this exception is due to the compare test failing.
        /// </summary>
        public virtual bool IsTestFailed => false;

        /// <summary>
        ///     Indicates that this exception is due to an HTTP timeout error.
        /// </summary>
        public virtual bool IsTimeout => false;

        /// <summary>
        ///     Indicates that this exception is due to the etcd being unable to parse the passed timeout value as a number.
        /// </summary>
        public virtual bool IsTimeoutNotANumber => false;

        /// <summary>
        ///     Indicates that this exception is due to etcd being unable to parse the passed ttl value as a number.
        /// </summary>
        public virtual bool IsTtlNotANumber => false;

        /// <summary>
        ///     Indicates that this exception is due to an unknown error.
        /// </summary>
        public virtual bool IsUnknown => false;

        /// <summary>
        ///     Indicates that this exception is due to the value or ttl field missing in the form post.
        /// </summary>
        public virtual bool IsValueOrTtlRequired => false;

        /// <summary>
        ///     Indicates that this exception is due to the value field missing in the form post.
        /// </summary>
        public virtual bool IsValueRequired => false;

        /// <summary>
        ///     Indicates that this exception is due to the watcher being cleared as a result of etcd recovery.
        /// </summary>
        public virtual bool IsWatcherCleared => false;

        /// <summary>
        ///     The request method for the operation.
        /// </summary>
        public HttpMethod RequestMethod { get; internal set; } = HttpMethod.Get;

        /// <summary>
        ///     The request url for the operation.
        /// </summary>
        public string RequestUrl { get; internal set; } = string.Empty;

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            if (EtcdError != null)
            {
                info.AddValue(nameof(EtcdError), JsonConvert.SerializeObject(EtcdError));
            }
            if (HttpStatusCode != null)
            {
                info.AddValue(nameof(HttpStatusCode), HttpStatusCode.Value.ToString());
            }
            if (RequestMethod != null)
            {
                info.AddValue(nameof(RequestMethod), RequestMethod.Method);
            }
            if (RequestUrl != null)
            {
                info.AddValue(nameof(RequestUrl), RequestUrl);
            }

            // Must call through to the base class to let it save it's own state
            base.GetObjectData(info, context);
        }
    }
}