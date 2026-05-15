using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Utils;

namespace UtilsUnitTest
{
    [TestClass]
    public class StringByteUtilsTest
    {
        #region BytesToString / StringToBytes

        [TestMethod]
        public void BytesToString_Empty_ReturnsEmpty()
        {
            Assert.AreEqual("", StringByteUtils.BytesToString([]));
            Assert.AreEqual("", StringByteUtils.BytesToString([0x01], 0));
        }

        [TestMethod]
        public void BytesToString_RoundTrip()
        {
            byte[] data = [0x01, 0xAB, 0xFF, 0x00];
            string s = StringByteUtils.BytesToString(data);
            Assert.AreEqual("01 AB FF 00", s);

            byte[] back = StringByteUtils.StringToBytes(s);
            CollectionAssert.AreEqual(data, back);
        }

        [TestMethod]
        public void BytesToString_WithCount()
        {
            byte[] data = [0x01, 0x02, 0x03];
            Assert.AreEqual("01 02", StringByteUtils.BytesToString(data, 2));
        }

        [TestMethod]
        public void BytesToString_CountExceedsLength_Throws()
        {
            try
            {
                StringByteUtils.BytesToString([0x01], 5);
                Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void StringToBytes_With0xAndSpaces()
        {
            var bytes = StringByteUtils.StringToBytes("0x01 0x02 0x03");
            CollectionAssert.AreEqual(new byte[] { 0x01, 0x02, 0x03 }, bytes);
        }

        [TestMethod]
        public void StringToBytes_InvalidHex_Throws()
        {
            try
            {
                StringByteUtils.StringToBytes("0x1G");
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void StringToBytes_OddLength_Throws()
        {
            try
            {
                StringByteUtils.StringToBytes("0x123");
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void ByteToString()
        {
            Assert.AreEqual("00", StringByteUtils.ByteToString(0x00));
            Assert.AreEqual("0A", StringByteUtils.ByteToString(0x0A));
            Assert.AreEqual("FF", StringByteUtils.ByteToString(0xFF));
        }

        #endregion

        #region CombineByteArray

        [TestMethod]
        public void CombineByteArray_Empty()
        {
            var result = StringByteUtils.CombineByteArray();
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void CombineByteArray_Single()
        {
            byte[] a = [0x01, 0x02];
            var result = StringByteUtils.CombineByteArray(a);
            CollectionAssert.AreEqual(a, result);
        }

        [TestMethod]
        public void CombineByteArray_Multiple()
        {
            byte[] a = [0x01, 0x02];
            byte[] b = [0x03];
            byte[] c = [0x04, 0x05, 0x06];
            var result = StringByteUtils.CombineByteArray(a, b, c);
            CollectionAssert.AreEqual(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, result);
        }

        [TestMethod]
        public void CombineByteArray_IEnumerable()
        {
            List<byte[]> list = new() { new byte[] { 0x01 }, new byte[] { 0x02, 0x03 } };
            var result = StringByteUtils.CombineByteArray(list);
            CollectionAssert.AreEqual(new byte[] { 0x01, 0x02, 0x03 }, result);
        }

        #pragma warning disable CS0618
        [TestMethod]
        public void ComibeByteArray_ObsoleteStillWorks()
        {
            var result = StringByteUtils.ComibeByteArray([0x01], [0x02]);
            CollectionAssert.AreEqual(new byte[] { 0x01, 0x02 }, result);
        }
        #pragma warning restore CS0618

        #endregion

        #region ValueEqual

        [TestMethod]
        public void ValueEqual_BothNull()
        {
            Assert.IsTrue(((byte[]?)null).ValueEqual(null));
        }

        [TestMethod]
        public void ValueEqual_OneNull()
        {
            Assert.IsFalse(((byte[]?)null).ValueEqual(new byte[] { 0x01 }));
            Assert.IsFalse((new byte[] { 0x01 }).ValueEqual(null));
        }

        [TestMethod]
        public void ValueEqual_DifferentLength()
        {
            Assert.IsFalse((new byte[] { 0x01 }).ValueEqual(new byte[] { 0x01, 0x02 }));
        }

        [TestMethod]
        public void ValueEqual_SameContent()
        {
            Assert.IsTrue((new byte[] { 0x01, 0x02 }).ValueEqual(new byte[] { 0x01, 0x02 }));
        }

        [TestMethod]
        public void ValueEqual_DifferentContent()
        {
            Assert.IsFalse((new byte[] { 0x01, 0x02 }).ValueEqual(new byte[] { 0x01, 0x03 }));
        }

        #endregion

        #region Numeric Conversions

        [TestMethod]
        public void ToInt16_LittleEndian()
        {
            byte[] data = [0x01, 0x02]; // 0x0201 = 513
            Assert.AreEqual((short)0x0201, StringByteUtils.ToInt16(data, 0));
        }

        [TestMethod]
        public void ToInt16_BigEndian()
        {
            byte[] data = [0x01, 0x02]; // 0x0102 = 258
            Assert.AreEqual((short)0x0102, StringByteUtils.ToInt16(data, 0, true));
        }

        [TestMethod]
        public void ToInt32_LittleEndian()
        {
            byte[] data = [0x01, 0x02, 0x03, 0x04]; // 0x04030201
            Assert.AreEqual(0x04030201, StringByteUtils.ToInt32(data, 0));
        }

        [TestMethod]
        public void ToInt32_BigEndian()
        {
            byte[] data = [0x01, 0x02, 0x03, 0x04]; // 0x01020304
            Assert.AreEqual(0x01020304, StringByteUtils.ToInt32(data, 0, true));
        }

        [TestMethod]
        public void ToInt64_LittleEndian()
        {
            byte[] data = [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08];
            Assert.AreEqual(0x0807060504030201L, StringByteUtils.ToInt64(data, 0));
        }

        [TestMethod]
        public void ToSingle_LittleEndian()
        {
            byte[] data = BitConverter.GetBytes(3.14f);
            Assert.AreEqual(3.14f, StringByteUtils.ToSingle(data, 0));
        }

        [TestMethod]
        public void ToSingle_BigEndian()
        {
            byte[] data = BitConverter.GetBytes(3.14f);
            Array.Reverse(data);
            Assert.AreEqual(3.14f, StringByteUtils.ToSingle(data, 0, true));
        }

        [TestMethod]
        public void ToDouble_LittleEndian()
        {
            byte[] data = BitConverter.GetBytes(3.1415926535);
            Assert.AreEqual(3.1415926535, StringByteUtils.ToDouble(data, 0));
        }

        [TestMethod]
        public void ToDouble_BigEndian()
        {
            byte[] data = BitConverter.GetBytes(3.1415926535);
            Array.Reverse(data);
            Assert.AreEqual(3.1415926535, StringByteUtils.ToDouble(data, 0, true));
        }

        [TestMethod]
        public void ToUInt16_LittleEndian()
        {
            byte[] data = [0xFF, 0xFF];
            Assert.AreEqual((ushort)0xFFFF, StringByteUtils.ToUInt16(data, 0));
        }

        [TestMethod]
        public void ToUInt32_LittleEndian()
        {
            byte[] data = [0xFF, 0xFF, 0xFF, 0xFF];
            Assert.AreEqual((uint)0xFFFFFFFF, StringByteUtils.ToUInt32(data, 0));
        }

        [TestMethod]
        public void ToUInt64_LittleEndian()
        {
            byte[] data = [0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF];
            Assert.AreEqual((ulong)0xFFFFFFFFFFFFFFFF, StringByteUtils.ToUInt64(data, 0));
        }

        [TestMethod]
        public void GetBytes_Int16_BigEndian()
        {
            var bytes = StringByteUtils.GetBytes((short)0x0102, true);
            CollectionAssert.AreEqual(new byte[] { 0x01, 0x02 }, bytes);
        }

        [TestMethod]
        public void GetBytes_Int32_BigEndian()
        {
            var bytes = StringByteUtils.GetBytes(0x01020304, true);
            CollectionAssert.AreEqual(new byte[] { 0x01, 0x02, 0x03, 0x04 }, bytes);
        }

        [TestMethod]
        public void GetBytes_Float_BigEndian()
        {
            var bytesLE = BitConverter.GetBytes(3.14f);
            var bytesBE = StringByteUtils.GetBytes(3.14f, true);
            Array.Reverse(bytesLE);
            CollectionAssert.AreEqual(bytesLE, bytesBE);
        }

        [TestMethod]
        public void GetBytes_Double_BigEndian()
        {
            var bytesLE = BitConverter.GetBytes(3.14159265);
            var bytesBE = StringByteUtils.GetBytes(3.14159265, true);
            Array.Reverse(bytesLE);
            CollectionAssert.AreEqual(bytesLE, bytesBE);
        }

        #endregion

        #region Span Overloads

        [TestMethod]
        public void Span_ToInt16_BigEndian()
        {
            ReadOnlySpan<byte> data = new byte[] { 0x01, 0x02 };
            Assert.AreEqual((short)0x0102, StringByteUtils.ToInt16(data, 0, true));
        }

        [TestMethod]
        public void Span_ToInt32_BigEndian()
        {
            ReadOnlySpan<byte> data = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            Assert.AreEqual(0x01020304, StringByteUtils.ToInt32(data, 0, true));
        }

        #endregion

        #region ToString

        [TestMethod]
        public void ToString_FullArray()
        {
            byte[] data = [0x01, 0x02, 0x03];
            Assert.AreEqual("01-02-03", StringByteUtils.ToString(data));
        }

        [TestMethod]
        public void ToString_WithStartIndex()
        {
            byte[] data = [0x01, 0x02, 0x03];
            Assert.AreEqual("02-03", StringByteUtils.ToString(data, 1));
        }

        [TestMethod]
        public void ToString_WithStartIndexAndLength()
        {
            byte[] data = [0x01, 0x02, 0x03, 0x04];
            Assert.AreEqual("02-03", StringByteUtils.ToString(data, 1, 2));
        }

        [TestMethod]
        public void ToString_BigEndian_Reverses()
        {
            byte[] data = [0x01, 0x02, 0x03];
            Assert.AreEqual("03-02-01", StringByteUtils.ToString(data, true));
        }

        #endregion
    }
}
