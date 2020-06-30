using System;
using System.Runtime.Serialization;

namespace Utils.PushQueue
{
    [Serializable]
    internal class MaxCacheCountOutOfRangeException : Exception
    {
        public MaxCacheCountOutOfRangeException()
        {
        }

        public MaxCacheCountOutOfRangeException(string message) : base(message)
        {
        }

        public MaxCacheCountOutOfRangeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MaxCacheCountOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}