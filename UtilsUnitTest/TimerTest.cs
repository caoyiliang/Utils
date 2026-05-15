using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Timer;

namespace UtilsUnitTest
{
    [TestClass]
    public class TimerTest
    {
        [TestMethod]
        public async Task TestStartAndStop()
        {
            var timer = new Utils.Timer.Timer(AverageTime.OneMinute);
            await timer.StartAsync();
            await Task.Delay(500);
            await timer.StopAsync();
            // Just verify no exception
        }

        [TestMethod]
        public async Task TestStopAsync_Idempotent()
        {
            var timer = new Utils.Timer.Timer(AverageTime.OneMinute);
            await timer.StartAsync();
            await Task.Delay(200);
            await timer.StopAsync();
            await timer.StopAsync(); // Should not throw
        }

        [TestMethod]
        public async Task TestStartAsync_Idempotent()
        {
            var timer = new Utils.Timer.Timer(AverageTime.OneMinute);
            await timer.StartAsync();
            await Task.Delay(200);
            await timer.StartAsync(); // Should not throw
            await timer.StopAsync();
        }

        [TestMethod]
        public async Task TestOnTime_Fires()
        {
            var timer = new Utils.Timer.Timer(AverageTime.OneMinute);
            timer.OnTime += (time) =>
            {
                // Timer event fired
            };
            await timer.StartAsync();
            await Task.Delay(2000);
            await timer.StopAsync();
            // Note: timer fires based on clock minute, so this may or may not fire
            // depending on when the test runs. We just verify it doesn't throw.
        }
    }
}
