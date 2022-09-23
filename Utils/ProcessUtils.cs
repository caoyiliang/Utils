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
    /// <param name="cancellationTokenSource">取消</param>
    /// <returns>可空T</returns>
    /// <exception cref="MultipleRetryFailureException">多次重试失败</exception>
    /// <exception cref="ArgumentOutOfRangeException">重试次数超范围</exception>
    public static async Task<T?> ReTry<T>(Func<Task<T>> func, int reTryCount = 0, CancellationTokenSource? cancellationTokenSource = null)
    {
        if (reTryCount < 0) throw new ArgumentOutOfRangeException(nameof(reTryCount));
        int j = -1;
        while (cancellationTokenSource == null || !cancellationTokenSource.IsCancellationRequested)
        {
            j++;
            if (j > reTryCount) break;
            try
            {
                T? rsp = default;
                if (func != null)
                    rsp = await func();
                return rsp;
            }
            catch (Exception)
            {
            }
        }
        throw new MultipleRetryFailureException();
    }

    /// <summary>
    /// 重试
    /// </summary>
    /// <param name="func">需要重试的方法</param>
    /// <param name="reTryCount">需要重试的次数</param>
    /// <param name="cancellationTokenSource">取消</param>
    /// <exception cref="MultipleRetryFailureException">多次重试失败</exception>
    /// <exception cref="ArgumentOutOfRangeException">重试次数超范围</exception>
    public static async Task ReTry(Func<Task> func, int reTryCount = 0, CancellationTokenSource? cancellationTokenSource = null)
    {
        if (reTryCount < 0) throw new ArgumentOutOfRangeException(nameof(reTryCount));
        int j = -1;
        while (cancellationTokenSource == null || !cancellationTokenSource.IsCancellationRequested)
        {
            j++;
            if (j > reTryCount) break;
            try
            {
                if (func != null)
                    await func();
                return;
            }
            catch (Exception)
            {
            }
        }
        throw new MultipleRetryFailureException();
    }
}
