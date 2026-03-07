using System;
using System.Runtime.Serialization;

namespace Draft.Exceptions
{
    /// <summary>
    ///     Represents an error with the underlying http client when attempting to connect to an etcd endpoint.
    /// </summary>
    [Serializable]
    public class HttpConnectionException : EtcdException
    {

        /// <summary>
        ///     Initializes a new <see cref="HttpConnectionException" /> instance.
        /// </summary>
        public HttpConnectionException() { }

        /// <summary>
        ///     Initializes a new <see cref="HttpConnectionException" /> instance with a specified error message.
        /// </summary>
        public HttpConnectionException(string message) : base(message) { }

        /// <summary>
        ///     Initializes a new <see cref="HttpConnectionException" /> instance for use in BCL deserialization
        /// </summary>
        protected HttpConnectionException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        ///     Indicates that this exception is due to an underlying http client connection error.
        /// </summary>
        public override bool IsHttpConnection => true;

    }
}
