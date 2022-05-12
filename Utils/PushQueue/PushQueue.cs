using System.Collections.Concurrent;

namespace Utils.PushQueue
{
    /// <summary>
    /// 队列
    /// </summary>
    /// <typeparam name="T">队列类型</typeparam>
    public class PushQueue<T>
    {
        private readonly ConcurrentQueue<T> _queue = new();
        private volatile bool _isActive = false;
        private TaskCompletionSource<bool>? _completeStop;
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
        /// <returns></returns>
        public async Task StartAsync()
        {
            if (_isActive) return;
            _isActive = true;
            _cts = new CancellationTokenSource();
            _ = Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    if (_queue.TryDequeue(out var t))
                    {
                        if (this.OnPushData is not null)
                            await OnPushData(t);
                    }
                    await Task.Delay(100);
                }
                _completeStop?.TrySetResult(true);
            });
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
        public async Task StopAsync()
        {
            if (!_isActive) return;
            _completeStop = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            _cts?.Cancel();
            await _completeStop.Task;
            _isActive = false;
        }
    }
}
