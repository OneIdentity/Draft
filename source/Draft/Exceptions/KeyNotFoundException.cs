using System;
using System.Runtime.Serialization;

namespace Draft.Exceptions
{
    /// <summary>
    ///     Etcd key not found exception
    /// </summary>
    [Serializable]
    public class KeyNotFoundException : EtcdException
    {

        /// <summary>
        ///     Initializes a new <see cref="KeyNotFoundException" /> instance.
        /// </summary>
        public KeyNotFoundException() { }

        /// <summary>
        ///     Initializes a new <see cref="KeyNotFoundException" /> instance with a specified error message.
        /// </summary>
        public KeyNotFoundException(string message) : base(message) { }

        /// <summary>
        ///     Initializes a new <see cref="KeyNotFoundException" /> instance for use in BCL deserialization
        /// </summary>
        protected KeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        ///     Indicates that this exception is due to the passed keyspace key not existing.
        /// </summary>
        public override bool IsKeyNotFound => true;

    }
}
