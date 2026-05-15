namespace Utils.Exceptions;

/// <summary>
/// 缓存队列数量超出最大值
/// </summary>
public class MaxCacheCountOutOfRangeException : Exception
{
    public MaxCacheCountOutOfRangeException() { }

    public MaxCacheCountOutOfRangeException(string message) : base(message) { }

    public MaxCacheCountOutOfRangeException(string message, Exception innerException) : base(message, innerException) { }
}
