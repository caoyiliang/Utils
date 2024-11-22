using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Timer;

namespace UtilsUnitTest
{
    [TestClass]
    public class TimerTest
    {
        [TestMethod]
        public async Task TestStartAsync()
        {
            Utils.Timer.ITimer timer = new Utils.Timer.Timer(AverageTime.OneMinute);
            timer.OnTime += time => Console.WriteLine($"{DateTime.Now}   {time}");
            await timer.StartAsync();
            await Task.Delay(5 * 60 * 1000);
        }

        [TestMethod]
        public async Task TestStopAsync()
        {
            Utils.Timer.ITimer timer = new Utils.Timer.Timer(AverageTime.OneMinute);
            await timer.StartAsync();
            await Task.Delay(2000);
            await timer.StopAsync();
        }
    }
}
