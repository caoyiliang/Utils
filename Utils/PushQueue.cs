using System.Threading.Channels;
using Utils.Exceptions;

namespace Utils;

/// <summary>
/// 队列
/// </summary>
/// <typeparam name="T">队列类型</typeparam>
public class PushQueue<T>
{
    private readonly Channel<T> _channel = Channel.CreateUnbounded<T>();
    private volatile bool _isActive = false;
    private Task? _task;
    private CancellationTokenSource? _cts;
    /// <summary>
    /// 最大缓存的数量
    /// </summary>
    public int MaxCacheCount { get; set; }
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
        _task = Task.Run(ProcessQueueAsync, _cts.Token);
        await Task.CompletedTask;
    }

    private async Task ProcessQueueAsync()
    {
        try
        {
            await foreach (var data in _channel.Reader.ReadAllAsync(_cts!.Token))
            {
                if (OnPushData is not null)
                    await OnPushData.Invoke(data);
            }
        }
        catch (OperationCanceledException)
        {
            // ignored
        }
    }

    /// <summary>
    /// 加入队列
    /// </summary>
    /// <param name="t"></param>
    /// <exception cref="MaxCacheCountOutOfRangeException"></exception>
    public async Task PutInDataAsync(T t)
    {
        if (_channel.Reader.Count > MaxCacheCount)
            throw new MaxCacheCountOutOfRangeException($"缓存队列中的数量超出最大值，最大值为{MaxCacheCount}");
        await _channel.Writer.WriteAsync(t);
    }

    /// <summary>
    /// 队列清理
    /// </summary>
    public void Clear()
    {
        while (_channel.Reader.TryRead(out _)) { }
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
