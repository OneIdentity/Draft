using System;
using System.Runtime.Serialization;

namespace Draft.Exceptions
{
    /// <summary>
    ///     Represents an error due to a pre-existing key.
    /// </summary>
    [Serializable]
    public class NodeExistsException : EtcdException
    {

        /// <summary>
        ///     Initializes a new <see cref="NodeExistsException" /> instance.
        /// </summary>
        public NodeExistsException() { }

        /// <summary>
        ///     Initializes a new <see cref="NodeExistsException" /> instance with a specified error message.
        /// </summary>
        public NodeExistsException(string message) : base(message) { }

        /// <summary>
        ///     Initializes a new <see cref="NodeExistsException" /> instance for use in BCL deserialization
        /// </summary>
        protected NodeExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        ///     Indicates that this exception is due to the passed key already existing.
        /// </summary>
        public override bool IsNodeExists => true;

    }
}
