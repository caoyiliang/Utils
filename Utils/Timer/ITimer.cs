using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Timer
{
    public interface ITimer
    {
        event Action<DateTime> OnTime;
        Task StartAsync();

        Task StopAsync();
    }
}
