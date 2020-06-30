using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CUtils.Timer
{
    public interface ITimer
    {
        event Action<DateTime> OnTime;
        Task StartAsync();

        Task StopAsync();
    }
}
