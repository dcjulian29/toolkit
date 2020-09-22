using System;
using System.Runtime.Serialization;

namespace ToolKit.DirectoryServices
{
    /// <summary>
    /// The exception that is thrown when an invalid distinguished name is parsed.
    /// </summary>
    [Serializable]
    public class InvalidDistinguishedNameException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDistinguishedNameException"/> class.
        /// </summary>
        public InvalidDistinguishedNameException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDistinguishedNameException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public InvalidDistinguishedNameException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDistinguishedNameException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public InvalidDistinguishedNameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDistinguishedNameException"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected InvalidDistinguishedNameException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
