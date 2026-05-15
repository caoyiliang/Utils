using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace UtilsUnitTest
{
    [TestClass]
    public class SignalToDigitalProcessorTest
    {
        [TestMethod]
        public void InputDataProcess_LinearMapping()
        {
            var processor = new SignalToDigitalProcessor(0, 10, 0, 100);
            Assert.AreEqual(50m, processor.InputDataProcess(5));
        }

        [TestMethod]
        public void InputDataProcess_MinBoundary()
        {
            var processor = new SignalToDigitalProcessor(0, 10, 0, 100);
            Assert.AreEqual(0m, processor.InputDataProcess(0));
            Assert.AreEqual(0m, processor.InputDataProcess(-5));
        }

        [TestMethod]
        public void InputDataProcess_MaxBoundary()
        {
            var processor = new SignalToDigitalProcessor(0, 10, 0, 100);
            Assert.AreEqual(100m, processor.InputDataProcess(10));
            Assert.AreEqual(100m, processor.InputDataProcess(15));
        }

        [TestMethod]
        public void OutputDataProcess_LinearMapping()
        {
            var processor = new SignalToDigitalProcessor(0, 10, 0, 100);
            Assert.AreEqual(5m, processor.OutputDataProcess(50));
        }

        [TestMethod]
        public void OutputDataProcess_MinBoundary()
        {
            var processor = new SignalToDigitalProcessor(0, 10, 0, 100);
            Assert.AreEqual(0m, processor.OutputDataProcess(0));
            Assert.AreEqual(0m, processor.OutputDataProcess(-10));
        }

        [TestMethod]
        public void OutputDataProcess_MaxBoundary()
        {
            var processor = new SignalToDigitalProcessor(0, 10, 0, 100);
            Assert.AreEqual(10m, processor.OutputDataProcess(100));
            Assert.AreEqual(10m, processor.OutputDataProcess(200));
        }

        [TestMethod]
        public void OutputDataProcess_MiddleValue()
        {
            var processor = new SignalToDigitalProcessor(4, 20, 0, 1600);
            // scale = (20-4)/1600 = 0.01
            Assert.AreEqual(10m, processor.OutputDataProcess(600)); // 4 + 0.01 * 600 = 10
        }
    }
}
