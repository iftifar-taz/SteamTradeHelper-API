using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SteamTradeHelper.Utilities.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The entity is not complete exception.
    /// </summary>
    [Serializable]
    public class EmptyListException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyListException"/> class.
        /// </summary>
        public EmptyListException()
            : base("List should not be empty.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyListException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public EmptyListException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyListException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">inner Exception. Instance of the <see cref="Exception"/> class.</param>
        public EmptyListException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyListException"/> class.
        /// </summary>
        /// <param name="info">info. Instance of the <see cref="SerializationInfo"/> class.</param>
        /// <param name="context">context. Instance of the <see cref="StreamingContext"/> class.</param>
        protected EmptyListException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
