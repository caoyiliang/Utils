using System.Runtime.Serialization;

namespace Utils.Exceptions
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
    }
}