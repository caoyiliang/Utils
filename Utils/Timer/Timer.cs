namespace Utils.Timer;

/// <summary>
/// 定时器
/// </summary>
/// <param name="averageTime"></param>
/// <param name="offset"></param>
public class Timer(AverageTime averageTime, int offset = 0) : ITimer
{
    private readonly int _minutes = (int)averageTime;
    private Task? _work;
    private CancellationTokenSource? _cts;
    private volatile bool _alive;

    public event Action<DateTime>? OnTime;

    public async Task StartAsync()
    {
        if (_alive) return;
        _alive = true;
        _cts = new CancellationTokenSource();
        var lastStartTime = DateTime.Now;
        _work = Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                var now = DateTime.Now;
                if (lastStartTime.Minute != now.Minute && now.Minute % _minutes == 0)
                {
                    lastStartTime = now;
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await Task.Delay(offset, _cts.Token);
                            OnTime?.Invoke(new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0));
                        }
                        catch (TaskCanceledException)
                        {
                            // Ignore the exception if the task was cancelled
                        }
                    }, _cts.Token);
                }
                try
                {
                    await Task.Delay(900, _cts.Token);
                }
                catch (TaskCanceledException)
                {
                    // Ignore the exception if the task was cancelled
                }
            }
        }, _cts.Token);
        await Task.CompletedTask;
    }

    public async Task StopAsync()
    {
        if (!_alive) return;
        _cts?.Cancel();
        if (_work != null)
        {
            try
            {
                await _work;
            }
            catch (TaskCanceledException)
            {
                // Ignore the exception if the task was cancelled
            }
        }
        _alive = false;
    }
}
