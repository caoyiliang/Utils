using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace UtilsUnitTest
{
    [TestClass]
    public class PushQueueTest
    {
        [TestMethod]
        public async Task TestStopAndRestartQueue()
        {
            // 初始化
            var pushQueue = new PushQueue<int>();
            pushQueue.MaxCacheCount = 100;
            int count = 0;
            pushQueue.OnPushData += async (data) =>
            {
                await Task.Delay(1000);
                count++;
                await Task.CompletedTask;
            };

            for (int i = 0; i < 10; i++)
            {
                pushQueue.PutInData(i);
            }
            // 启动队列
            await pushQueue.StartAsync();
            await Task.Delay(2000);
            // 停止队列
            await pushQueue.StopAsync();
            await Task.Delay(2000);
            // 再次启动队列
            await pushQueue.StartAsync();



            // 等待一段时间以确保数据被处理
            await Task.Delay(15000);

            // 验证
            Assert.IsTrue(count == 10, "队列在停止后再次启动，应能正常处理数据。");
        }
    }
}
