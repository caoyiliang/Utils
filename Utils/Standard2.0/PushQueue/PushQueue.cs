using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CUtils.PushQueue
{
    public class PushQueue<T>
    {
        private ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private volatile bool _isActive = false;
        private TaskCompletionSource<bool> _completeStop;
        private CancellationTokenSource _cts;
        /// <summary>
        /// 最大缓存的数量
        /// </summary>
        public int MaxCacheCount { get; set; }
        public event Func<T, Task> OnPushData;
        public async Task StartAsync()
        {
            if (_isActive) return;
            _isActive = true;
            _cts = new CancellationTokenSource();
            _ = Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    if (_queue.TryDequeue(out T t))
                    {
                        if (!(this.OnPushData is null))
                            try
                            {
                                await OnPushData(t);
                            }
                            catch 
                            {
                            
                            }
                    }
                    await Task.Delay(100);
                }
                _completeStop.TrySetResult(true);
            });
        }
        public void PutInData(T t)
        {
            if (_queue.Count > MaxCacheCount)
                throw new MaxCacheCountOutOfRangeException($"缓存队列中的数量超出最大值，最大值为{MaxCacheCount}");
            _queue.Enqueue(t);
        }
        public async Task StopAsync()
        {
            if (!_isActive) return;
            _isActive = false;
            _completeStop = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            _cts.Cancel();
            await _completeStop.Task;
        }
    }
}
