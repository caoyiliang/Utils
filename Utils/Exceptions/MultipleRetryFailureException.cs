using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Exceptions
{
    /// <summary>
    /// 多次重试失败
    /// </summary>
    public class MultipleRetryFailureException : Exception
    {
        /// <summary>多次重试失败</summary>
        public MultipleRetryFailureException() : base() { }
        /// <summary>多次重试失败</summary>
        public MultipleRetryFailureException(string message) : base(message) { }
        /// <summary>多次重试失败</summary>
        public MultipleRetryFailureException(string message, Exception innerException) : base(message, innerException) { }
    }
}
