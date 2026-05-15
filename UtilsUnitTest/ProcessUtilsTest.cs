using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using Utils.Exceptions;

namespace UtilsUnitTest
{
    [TestClass]
    public class ProcessUtilsTest
    {
        [TestMethod]
        public async Task ReTry_SucceedsFirstTime()
        {
            int count = 0;
            var result = await (new Func<Task<int>>(() =>
            {
                count++;
                return Task.FromResult(42);
            })).ReTry(3);
            Assert.AreEqual(42, result);
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public async Task ReTry_SucceedsOnRetry()
        {
            int count = 0;
            var result = await (new Func<Task<int>>(() =>
            {
                count++;
                if (count < 3)
                    throw new InvalidOperationException("fail");
                return Task.FromResult(99);
            })).ReTry(5);
            Assert.AreEqual(99, result);
            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public async Task ReTry_AlwaysFails_Throws()
        {
            try
            {
                await (new Func<Task<int>>(() => throw new InvalidOperationException("fail"))).ReTry(2);
                Assert.Fail("Expected MultipleRetryFailureException was not thrown.");
            }
            catch (MultipleRetryFailureException)
            {
            }
        }

        [TestMethod]
        public async Task ReTry_NegativeCount_Throws()
        {
            try
            {
                await (new Func<Task<int>>(() => Task.FromResult(1))).ReTry(-1);
                Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public async Task ReTry_Cancellation_ThrowsImmediately()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();
            try
            {
                await (new Func<Task<int>>(() => Task.FromResult(1))).ReTry(3, cts.Token);
                Assert.Fail("Expected OperationCanceledException was not thrown.");
            }
            catch (OperationCanceledException)
            {
            }
        }

        [TestMethod]
        public async Task ReTry_Void_Succeeds()
        {
            int count = 0;
            await (new Func<Task>(() =>
            {
                count++;
                return Task.CompletedTask;
            })).ReTry(2);
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public async Task ReTry_Void_AlwaysFails_Throws()
        {
            try
            {
                await (new Func<Task>(() => throw new InvalidOperationException("fail"))).ReTry(1);
                Assert.Fail("Expected MultipleRetryFailureException was not thrown.");
            }
            catch (MultipleRetryFailureException)
            {
            }
        }

        [TestMethod]
        public async Task ReTry_ExceptionPreserved()
        {
            var originalEx = new InvalidOperationException("specific");
            try
            {
                await (new Func<Task<int>>(() => throw originalEx)).ReTry(0);
            }
            catch (MultipleRetryFailureException ex) when (ex.InnerException is InvalidOperationException inner)
            {
                Assert.AreEqual("specific", inner.Message);
                return;
            }
            Assert.Fail("Expected MultipleRetryFailureException with inner exception");
        }
    }
}
