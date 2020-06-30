using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using CUtils.Timer;

namespace UtilsUnitTest
{
    [TestClass]
    public class TimerTest
    {
        [TestMethod]
        public async Task TestStartAsync()
        {
            ITimer timer = new Timer(AverageTime.OneMinute);
            timer.OnTime += time => Console.WriteLine($"{DateTime.Now}   {time}");
            await timer.StartAsync();
            await Task.Delay(5 * 60 * 1000);
        }
        [TestMethod]
        public async Task TestStopAsync()
        {
            ITimer timer = new Timer(AverageTime.OneMinute);
            await timer.StartAsync();
            await Task.Delay(2000);
            await timer.StopAsync();
        }
    }
}
