using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using Utils.Exceptions;

namespace UtilsUnitTest
{
    [TestClass]
    public class PushQueueTest
    {
        [TestMethod]
        public async Task TestStopAndRestartQueue()
        {
            var pushQueue = new PushQueue<int>();
            pushQueue.MaxCacheCount = 100;
            int count = 0;
            pushQueue.OnPushData += async (data) =>
            {
                await Task.Delay(100);
                count++;
                await Task.CompletedTask;
            };

            for (int i = 0; i < 10; i++)
            {
                await pushQueue.PutInDataAsync(i);
            }
            await pushQueue.StartAsync();
            await Task.Delay(2000);
            await pushQueue.StopAsync();
            await Task.Delay(200);
            await pushQueue.StartAsync();
            await Task.Delay(2000);
            await pushQueue.StopAsync();

            Assert.AreEqual(10, count, "队列在停止后再次启动，应能正常处理数据。");
        }

        [TestMethod]
        public async Task TestMaxCacheCount_Throws()
        {
            var queue = new PushQueue<int>();
            queue.MaxCacheCount = 2;

            await queue.PutInDataAsync(1);
            await queue.PutInDataAsync(2);

            try
            {
                await queue.PutInDataAsync(3);
                Assert.Fail("Expected MaxCacheCountOutOfRangeException was not thrown.");
            }
            catch (MaxCacheCountOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public async Task TestClear()
        {
            var queue = new PushQueue<int>();
            queue.MaxCacheCount = 100;
            for (int i = 0; i < 5; i++)
            {
                await queue.PutInDataAsync(i);
            }
            queue.Clear();
            // After Clear, putting more data should not throw
            await queue.PutInDataAsync(99);
        }

        [TestMethod]
        public async Task TestStartAsync_Idempotent()
        {
            var queue = new PushQueue<int>();
            await queue.StartAsync();
            // Second call should not throw
            await queue.StartAsync();
            await queue.StopAsync();
        }

        [TestMethod]
        public async Task TestStopAsync_Idempotent()
        {
            var queue = new PushQueue<int>();
            await queue.StartAsync();
            await queue.StopAsync();
            // Second stop should not throw
            await queue.StopAsync();
        }

        [TestMethod]
        public async Task TestDataProcessing()
        {
            var queue = new PushQueue<int>();
            queue.MaxCacheCount = 100;
            var processed = new List<int>();
            queue.OnPushData += async (data) =>
            {
                processed.Add(data);
                await Task.CompletedTask;
            };

            await queue.StartAsync();
            await queue.PutInDataAsync(1);
            await queue.PutInDataAsync(2);
            await queue.PutInDataAsync(3);
            await Task.Delay(500);
            await queue.StopAsync();

            CollectionAssert.AreEqual(new List<int> { 1, 2, 3 }, processed);
        }
    }
}
