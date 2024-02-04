using Utils.Exceptions;

namespace Utils;

public static class ProcessUtils
{
    /// <summary>
    /// 重试
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <param name="task">需要重试的方法</param>
    /// <param name="reTryCount">需要重试的次数</param>
    /// <param name="cancellationTokenSource">取消</param>
    /// <returns>可空T</returns>
    /// <exception cref="MultipleRetryFailureException">多次重试失败</exception>
    /// <exception cref="ArgumentOutOfRangeException">重试次数超范围</exception>
    /// <exception cref="TaskCanceledException">任务取消</exception>
    public static async Task<T?> ReTry<T>(this Task<T> task, int reTryCount = 0, CancellationTokenSource? cancellationTokenSource = null)
    {
        if (reTryCount < 0) throw new ArgumentOutOfRangeException(nameof(reTryCount));
        int j = -1;
        while (true)
        {
            if (cancellationTokenSource != null && cancellationTokenSource.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }
            j++;
            if (j > reTryCount) break;
            try
            {
                T? rsp = default;
                if (task != null)
                    rsp = await task;
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
    /// <param name="task">需要重试的方法</param>
    /// <param name="reTryCount">需要重试的次数</param>
    /// <param name="cancellationTokenSource">取消</param>
    /// <exception cref="MultipleRetryFailureException">多次重试失败</exception>
    /// <exception cref="ArgumentOutOfRangeException">重试次数超范围</exception>
    /// <exception cref="TaskCanceledException">任务取消</exception>
    public static async Task ReTry(this Task task, int reTryCount = 0, CancellationTokenSource? cancellationTokenSource = null)
    {
        if (reTryCount < 0) throw new ArgumentOutOfRangeException(nameof(reTryCount));
        int j = -1;
        while (true)
        {
            if (cancellationTokenSource != null && cancellationTokenSource.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }
            j++;
            if (j > reTryCount) break;
            try
            {
                if (task != null)
                    await task;
                return;
            }
            catch (Exception)
            {
            }
        }
        throw new MultipleRetryFailureException();
    }
}
