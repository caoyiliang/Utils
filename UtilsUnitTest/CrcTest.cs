using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace UtilsUnitTest
{
    [TestClass]
    public class CrcTest
    {
        private readonly byte[] _testData = [0x01, 0x02, 0x03, 0x04, 0x05];

        [TestMethod]
        public void Crc16_Length()
        {
            var crc = CRC.Crc16(_testData, _testData.Length);
            Assert.AreEqual(2, crc.Length);
        }

        [TestMethod]
        public void Crc16_WithData()
        {
            var result = CRC.Crc16(_testData);
            // 数据 + CRC
            Assert.AreEqual(_testData.Length + 2, result.Length);
            // 验证最后两个字节是 CRC
            var crcOnly = CRC.Crc16(_testData, _testData.Length);
            CollectionAssert.AreEqual(crcOnly, result.Skip(_testData.Length).ToArray());
        }

        [TestMethod]
        public void CRC16_R_Reverses()
        {
            var crc = CRC.Crc16(_testData, _testData.Length);
            var crcR = CRC.CRC16_R(_testData);
            CollectionAssert.AreEqual(crc, crcR.Reverse().ToArray());
        }

        [TestMethod]
        public void UpdateCRC_Length()
        {
            var crc = CRC.UpdateCRC(_testData, _testData.Length);
            var bytes = CRC.UpdateCRC(_testData);
            Assert.AreEqual(2, bytes.Length);
            // UpdateCRC(byte[]) returns big-endian bytes via StringByteUtils.GetBytes(crc, true)
            CollectionAssert.AreEqual(StringByteUtils.GetBytes(crc, true), bytes);
        }

        [TestMethod]
        public void GBcrc16_NotNull()
        {
            var crc = CRC.GBcrc16(_testData, _testData.Length);
            Assert.AreEqual(2, crc.Length);
        }

        [TestMethod]
        public void HBcrc16_NotNull()
        {
            var crc = CRC.HBcrc16(_testData, _testData.Length);
            Assert.AreEqual(2, crc.Length);
        }

        [TestMethod]
        public void Crc16_KnownValue()
        {
            byte[] data = [0x31, 0x32, 0x33, 0x34, 0x35]; // "12345"
            var crc = CRC.Crc16(data, data.Length);
            // 这是一个存在性测试，验证不会返回空或异常
            Assert.AreEqual(2, crc.Length);
        }
    }
}
