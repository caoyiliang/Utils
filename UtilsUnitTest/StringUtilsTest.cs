using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text;
using Utils;

namespace UtilsUnitTest
{
    [TestClass]
    public class StringUtilsTest
    {
        #region GetAllNum

        [TestMethod]
        public void GetAllNum_MixedContent()
        {
            var str = ">-3.2441-3.1245+3.2423+3.1817+3.1340+2.5312+2.2312-2.5312";
            var result = str.GetAllNum();
            Assert.AreEqual(8, result.Count);
            Assert.AreEqual(-3.2441m, result[0]);
            Assert.AreEqual(-3.1245m, result[1]);
            Assert.AreEqual(3.2423m, result[2]);
            Assert.AreEqual(-2.5312m, result[7]);
        }

        [TestMethod]
        public void GetAllNum_EmptyString()
        {
            var result = "".GetAllNum();
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetAllNum_NoNumbers()
        {
            var result = "abc xyz".GetAllNum();
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetAllNum_OnlyNumbers()
        {
            var result = "1 2 3".GetAllNum();
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void GetAllNum_NegativeOnly()
        {
            var result = "-5-3".GetAllNum();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(-5m, result[0]);
            Assert.AreEqual(-3m, result[1]);
        }

        [TestMethod]
        public void GetAllNum_DecimalNumbers()
        {
            var result = "1.5 2.75".GetAllNum();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1.5m, result[0]);
            Assert.AreEqual(2.75m, result[1]);
        }

        #endregion

        #region ModelToUriParam

        private class TestModel
        {
            public string Name { get; set; } = "test";
            public int Age { get; set; } = 25;
            public string? Empty { get; set; }
        }

        [TestMethod]
        public void ModelToUriParam_Basic()
        {
            var model = new TestModel();
            var uri = model.ModelToUriParam();
            Assert.IsTrue(uri.Contains("Age=25"));
            Assert.IsTrue(uri.Contains("Name=test"));
            Assert.IsFalse(uri.Contains("Empty"));
        }

        [TestMethod]
        public void ModelToUriParam_WithUrl()
        {
            var model = new TestModel();
            var uri = model.ModelToUriParam(url: "http://example.com");
            Assert.IsTrue(uri.StartsWith("http://example.com?"));
        }

        [TestMethod]
        public void ModelToUriParam_Sorted()
        {
            var model = new TestModel();
            var uri = model.ModelToUriParam(sort: true);
            var ageIndex = uri.IndexOf("Age");
            var nameIndex = uri.IndexOf("Name");
            Assert.IsTrue(ageIndex < nameIndex, "Age should come before Name when sorted");
        }

        [TestMethod]
        public void ModelToUriParam_UrlEncoded()
        {
            var model = new TestModel { Name = "hello world" };
            var uri = model.ModelToUriParam(urlEncode: true);
            Assert.IsTrue(uri.Contains("hello+world") || uri.Contains("hello%20world"));
        }

        [TestMethod]
        public void ModelToUriParam_EmptyObject()
        {
            var model = new object();
            var uri = model.ModelToUriParam();
            Assert.AreEqual("", uri);
        }

        #endregion

        #region ResolveHostAsync

        [TestMethod]
        public async Task ResolveHostAsync_IPAddress()
        {
            var ip = await "127.0.0.1".ResolveHostAsync();
            Assert.AreEqual(IPAddress.Parse("127.0.0.1"), ip);
        }

        [TestMethod]
        public async Task ResolveHostAsync_Localhost()
        {
            var ip = await "localhost".ResolveHostAsync();
            Assert.IsNotNull(ip);
        }

        [TestMethod]
        public async Task ResolveHostAsync_InvalidHost_Throws()
        {
            try
            {
                await "this-is-not-a-valid-host-123456789.local".ResolveHostAsync();
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (InvalidOperationException)
            {
            }
        }

        #endregion

        #region AES (legacy location)

        [TestMethod]
        public async Task TestAes()
        {
            using var aes = new AES(Encoding.UTF8.GetBytes("sciencetechnolog"), iv: Encoding.UTF8.GetBytes("sciencetechnolog"));
            var a = aes.Encrypt("adfasdf");
            var b = aes.Decrypt(a);
            Assert.AreEqual("adfasdf", b);
            await Task.CompletedTask;
        }

        #endregion

        #region MD5 (legacy location)

        [TestMethod]
        public async Task TestMD5()
        {
            var str = "123456";
            var md5 = MD5.GetMD5Hash(str);
            var result = MD5.VerifyMD5Hash(str, md5);
            Assert.IsTrue(result);
            await Task.CompletedTask;
        }

        #endregion

        #region CRC (legacy location)

        [TestMethod]
        public async Task TestCrc()
        {
            byte[] bytes = [0x01, 0x02, 0x03, 0x04, 0x05];
            var data = Encoding.ASCII.GetBytes("01 02 03 04 05");
            var crc16 = CRC.Crc16(bytes, bytes.Length);
            var crc16_r = CRC.CRC16_R(bytes);
            var updateCRC = CRC.UpdateCRC(bytes);
            var gb = CRC.GBcrc16(data, data.Length);
            var hb = CRC.HBcrc16(data, data.Length);
            Assert.AreEqual(2, crc16.Length);
            Assert.AreEqual(2, crc16_r.Length);
            Assert.AreEqual(2, updateCRC.Length);
            await Task.CompletedTask;
        }

        #endregion

        #region CombineByteArray (legacy location)

        [TestMethod]
        public async Task TestCombineByteArray()
        {
            byte[] a = [0x01, 0x02, 0x03];
            byte[] b = [0x04, 0x05, 0x06];
            var result = StringByteUtils.CombineByteArray(a, b);
            CollectionAssert.AreEqual(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, result);
            await Task.CompletedTask;
        }

        #endregion

        #region ValueEqual (legacy location)

        [TestMethod]
        public async Task TestValueEqual()
        {
            byte[]? a = null;
            byte[]? b = null;
            var result = a.ValueEqual(b);
            Assert.IsTrue(result);
            await Task.CompletedTask;
        }

        #endregion

        #region StringToBytes (legacy location)

        [TestMethod]
        public async Task TestStringToBytes()
        {
            string s = "0x08000000";
            var bytes = StringByteUtils.StringToBytes(s);
            Assert.AreEqual(4, bytes.Length);
            await Task.CompletedTask;
        }

        #endregion

        #region ReTry (legacy location)

        [TestMethod]
        public async Task TestReTry()
        {
            try
            {
                var func = () => TaskA();
                await func.ReTry(3);
                Assert.Fail("Expected MultipleRetryFailureException was not thrown.");
            }
            catch (Utils.Exceptions.MultipleRetryFailureException)
            {
            }
        }

        private static Task TaskA() => throw new Exception();

        #endregion
    }
}
