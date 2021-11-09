namespace Utils.Timer
{
    public class Timer : ITimer
    {
        private int _minutes;
        private Task _work;
        private TaskCompletionSource<bool> _stop;
        private volatile bool _alive;
        private readonly int _offset;

        public event Action<DateTime> OnTime;
        public Timer(AverageTime averageTime, int offset = 0)
        {
            _minutes = (int)averageTime;
            _offset = offset;
        }
        public async Task StartAsync()
        {
            if (_alive) return;
            _alive = true;
            _stop = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var lastStartTime = DateTime.Now;
            _work = Task.Run(async () =>
            {
                while (!_stop.Task.IsCompleted)
                {
                    var now = DateTime.Now;
                    if (lastStartTime.Minute != now.Minute && now.Minute % _minutes == 0)
                    {
                        lastStartTime = now;
                        _ = Task.Run(async () =>
                        {
                            await Task.Delay(_offset);
                            try
                            {
                                OnTime(new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0));
                            }
                            catch (Exception)
                            {
                            }
                        });
                    }
                    if (await Task.WhenAny(Task.Delay(900), _stop.Task) == _stop.Task) break;
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
