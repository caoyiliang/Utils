using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace UtilsUnitTest
{
    [TestClass]
    public class RemainBytesTest
    {
        [TestMethod]
        public void Append_IncreasesCount()
        {
            var rb = new RemainBytes();
            byte[] data = [0x01, 0x02, 0x03];
            rb.Append(data, 0, data.Length);
            Assert.AreEqual(3, rb.Count);
        }

        [TestMethod]
        public void Append_MultipleTimes()
        {
            var rb = new RemainBytes();
            rb.Append([0x01], 0, 1);
            rb.Append([0x02], 0, 1);
            rb.Append([0x03], 0, 1);
            Assert.AreEqual(3, rb.Count);
            Assert.AreEqual(0x01, rb.Bytes[rb.StartIndex]);
            Assert.AreEqual(0x03, rb.Bytes[rb.StartIndex + 2]);
        }

        [TestMethod]
        public void RemoveHeader_ShiftStartIndex()
        {
            var rb = new RemainBytes();
            rb.Append([0x01, 0x02, 0x03], 0, 3);
            rb.RemoveHeader(1);
            Assert.AreEqual(2, rb.Count);
            Assert.AreEqual(1, rb.StartIndex);
            Assert.AreEqual(0x02, rb.Bytes[rb.StartIndex]);
        }

        [TestMethod]
        public void SetCurrentMessageLength_CapacityAdjusted()
        {
            var rb = new RemainBytes();
            rb.SetCurrentMessageLength(100);
            rb.Append(new byte[50], 0, 50);
            Assert.IsTrue(rb.Capacity >= 100);
        }

        [TestMethod]
        public void Capacity_GrowsDynamically()
        {
            var rb = new RemainBytes();
            var largeData = new byte[10000];
            rb.Append(largeData, 0, largeData.Length);
            Assert.AreEqual(10000, rb.Count);
            Assert.IsTrue(rb.Capacity >= 10000);
        }

        [TestMethod]
        public void RemoveHeader_AllData_Removes()
        {
            var rb = new RemainBytes();
            rb.Append([0x01, 0x02], 0, 2);
            rb.RemoveHeader(2);
            Assert.AreEqual(0, rb.Count);
        }
    }
}
