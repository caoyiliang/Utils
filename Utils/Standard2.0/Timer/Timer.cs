using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.Timer
{
    public class Timer : ITimer
    {
        private int _minutes;
        private Task _work;
        private TaskCompletionSource<bool> _stop;
        private volatile bool _alive;
        public event Action<DateTime> OnTime;
        public Timer(AverageTime averageTime)
        {
            _minutes = (int)averageTime;
        }
        public async Task StartAsync()
        {
            if (_alive) return;
            _alive = true;
            _stop = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            _work = Task.Run(async () =>
            {
                while (true)
                {
                    var now = DateTime.Now;
                    if ((now.Minute - 1 == -1 ? 59 : now.Minute - 1) % _minutes == 0)
                    {
                        try
                        {
                            OnTime(new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0));
                        }
                        catch (Exception)
                        {
                        }
                        if (await Task.WhenAny(Task.Delay(_minutes * 60 * 1000 - now.Second * 1000), _stop.Task) == _stop.Task) break;
                    }
                    else
                    {
                        if (await Task.WhenAny(Task.Delay(900), _stop.Task) == _stop.Task) break;
                    }
                }
            });
        }

        public async Task StopAsync()
        {
            if (!_alive) return;
            _stop.TrySetResult(true);
            await _work;
            _alive = false;
        }
    }
}
