using System.Collections.Concurrent;
using Utils.Exceptions;

namespace Utils;

/// <summary>
/// 队列
/// </summary>
/// <typeparam name="T">队列类型</typeparam>
public class PushQueue<T>
{
    private readonly ConcurrentQueue<T> _queue = new();
    private volatile bool _isActive = false;
    private Task? _task;
    private CancellationTokenSource? _cts;
    /// <summary>
    /// 最大缓存的数量
    /// </summary>
    public int MaxCacheCount { get; set; }
    /// <summary>
    /// 队列循环时间
    /// </summary>
    public int DelayTime { get; set; } = 100;
    /// <summary>
    /// 队列推出事件
    /// </summary>
    public event Func<T, Task>? OnPushData;
    /// <summary>
    /// 启动队列
    /// </summary>
    public async Task StartAsync()
    {
        if (_isActive) return;
        _isActive = true;
        _cts = new CancellationTokenSource();
        _task = Task.Run(async () =>
        {
            while (!_cts.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var t))
                {
                    if (OnPushData is not null)
                        await OnPushData.Invoke(t);
                }
                else
                {
                    await Task.Delay(DelayTime, _cts.Token);
                }
            }
        }, _cts.Token);
        await Task.CompletedTask;
    }
    /// <summary>
    /// 加入队列
    /// </summary>
    /// <param name="t"></param>
    /// <exception cref="MaxCacheCountOutOfRangeException"></exception>
    public void PutInData(T t)
    {
        if (_queue.Count > MaxCacheCount)
            throw new MaxCacheCountOutOfRangeException($"缓存队列中的数量超出最大值，最大值为{MaxCacheCount}");
        _queue.Enqueue(t);
    }

    /// <summary>
    /// 队列清理
    /// </summary>
    public void Clear()
    {
#if NET6_0_OR_GREATER
        _queue.Clear();
#else
        while (_queue.TryDequeue(out _)) ;
#endif
    }

    /// <summary>
    /// 停止队列
    /// </summary>
    public async Task StopAsync()
    {
        if (!_isActive) return;
        _cts?.Cancel();
        if (_task is not null) await _task;
        _isActive = false;
    }
}
