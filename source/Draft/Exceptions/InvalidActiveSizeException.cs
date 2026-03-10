using System;
using System.Runtime.Serialization;

namespace Draft.Exceptions
{
    /// <summary>
    ///     Represents an "Invalid active size" error.
    /// </summary>
    [Serializable]
    public class InvalidActiveSizeException : EtcdException
    {

        /// <summary>
        ///     Initializes a new <see cref="InvalidActiveSizeException" /> instance.
        /// </summary>
        public InvalidActiveSizeException() { }

        /// <summary>
        ///     Initializes a new <see cref="InvalidActiveSizeException" /> instance with a specified error message.
        /// </summary>
        public InvalidActiveSizeException(string message) : base(message) { }

        /// <summary>
        ///     Initializes a new <see cref="InvalidActiveSizeException" /> instance for use in BCL deserialization
        /// </summary>
        protected InvalidActiveSizeException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        ///     Indicates that this exception is due to an "Invalid active size" error.
        /// </summary>
        public override bool IsInvalidActiveSize => true;

    }
}
