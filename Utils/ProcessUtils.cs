using Utils.Exceptions;

namespace Utils;

public static class ProcessUtils
{
    /// <summary>
    /// 重试
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <param name="func">需要重试的方法</param>
    /// <param name="reTryCount">需要重试的次数</param>
    /// <param name="cancellationToken">取消</param>
    /// <returns>可空T</returns>
    /// <exception cref="MultipleRetryFailureException">多次重试失败</exception>
    /// <exception cref="ArgumentOutOfRangeException">重试次数超范围</exception>
    /// <exception cref="OperationCanceledException">任务取消</exception>
    public static async Task<T?> ReTry<T>(this Func<Task<T>> func, int reTryCount = 0, CancellationToken cancellationToken = default)
    {
        if (reTryCount < 0) throw new ArgumentOutOfRangeException(nameof(reTryCount));
        Exception? lastException = null;
        for (int j = 0; j <= reTryCount; j++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                return await func.Invoke();
            }
            catch (Exception ex)
            {
                lastException = ex;
            }
        }
        throw new MultipleRetryFailureException("Multiple retries failed", lastException);
    }

    /// <summary>
    /// 重试
    /// </summary>
    /// <param name="func">需要重试的方法</param>
    /// <param name="reTryCount">需要重试的次数</param>
    /// <param name="cancellationToken">取消</param>
    /// <exception cref="MultipleRetryFailureException">多次重试失败</exception>
    /// <exception cref="ArgumentOutOfRangeException">重试次数超范围</exception>
    /// <exception cref="OperationCanceledException">任务取消</exception>
    public static async Task ReTry(this Func<Task> func, int reTryCount = 0, CancellationToken cancellationToken = default)
    {
        if (reTryCount < 0) throw new ArgumentOutOfRangeException(nameof(reTryCount));
        Exception? lastException = null;
        for (int j = 0; j <= reTryCount; j++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await func.Invoke();
                return;
            }
            catch (Exception ex)
            {
                lastException = ex;
            }
        }
        throw new MultipleRetryFailureException("Multiple retries failed", lastException);
    }
}
