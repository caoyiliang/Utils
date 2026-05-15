namespace Utils.Timer;

/// <summary>
/// 定时器
/// </summary>
public class Timer(AverageTime averageTime, int offset = 0) : ITimer
{
    private readonly int _minutes = (int)averageTime;
    private readonly object _lifecycleLock = new();
    private Task? _work;
    private volatile CancellationTokenSource? _cts;
    private volatile bool _alive;

    public event Action<DateTime>? OnTime;

    public Task StartAsync()
    {
        lock (_lifecycleLock)
        {
            if (_alive) return Task.CompletedTask;
            _alive = true;
            _cts = new CancellationTokenSource();
            _work = Task.Run(TimerLoopAsync, _cts.Token);
            return Task.CompletedTask;
        }
    }

    private async Task TimerLoopAsync()
    {
        while (!_cts!.Token.IsCancellationRequested)
        {
            var delay = CalculateDelayToNextTrigger();
            if (delay <= TimeSpan.Zero)
            {
                await TriggerAsync();
                // 防止同一分钟内重复触发
                if (await SafeDelayAsync(TimeSpan.FromSeconds(1))) return;
            }
            else
            {
                // 分段等待（每次最多 1 秒），系统时间变更后可立即重新计算 delay
                var chunk = TimeSpan.FromTicks(Math.Min(delay.Ticks, TimeSpan.TicksPerSecond));
                if (await SafeDelayAsync(chunk)) return;
            }
        }
    }

    private async Task TriggerAsync()
    {
        if (OnTime is null) return;
        try
        {
            if (offset > 0)
            {
                await Task.Delay(offset, _cts!.Token);
            }
            var now = DateTime.Now;
            OnTime.Invoke(new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0));
        }
        catch (OperationCanceledException)
        {
            // ignored
        }
        catch
        {
            // 事件处理异常不应导致定时器崩溃
        }
    }

    private TimeSpan CalculateDelayToNextTrigger()
    {
        var now = DateTime.Now;
        int currentMinute = now.Minute;
        int nextMinute = ((currentMinute / _minutes) + 1) * _minutes;
        var nextTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddMinutes(nextMinute);
        if (nextTime <= now)
        {
            nextTime = nextTime.AddHours(1);
        }
        return nextTime - now;
    }

    private async Task<bool> SafeDelayAsync(TimeSpan delay)
    {
        try
        {
            await Task.Delay(delay, _cts!.Token);
            return false;
        }
        catch (OperationCanceledException)
        {
            return true;
        }
    }

    public async Task StopAsync()
    {
        lock (_lifecycleLock)
        {
            if (!_alive) return;
            _cts?.Cancel();
        }
        if (_work != null)
        {
            try
            {
                await _work;
            }
            catch (OperationCanceledException)
            {
                // ignored
            }
        }
        lock (_lifecycleLock)
        {
            _alive = false;
        }
    }
}
