using Utils.Exceptions;

namespace Utils;

public static class ProcessUtils
{
    public static async Task<T?> ReTry<T>(Func<Task<T>> func, int i = -1, CancellationTokenSource? cancellationTokenSource = null)
    {
        int j = -1;
        while (cancellationTokenSource == null || !cancellationTokenSource.IsCancellationRequested)
        {
            j++;
            if ((i != -1) && (j >= i)) break;
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

    public static async Task ReTry(Func<Task> func, int i = -1, CancellationTokenSource? cancellationTokenSource = null)
    {
        int j = -1;
        while (cancellationTokenSource == null || !cancellationTokenSource.IsCancellationRequested)
        {
            j++;
            if ((i != -1) && (j >= i)) break;
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
