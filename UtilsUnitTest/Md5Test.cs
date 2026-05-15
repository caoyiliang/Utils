using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace UtilsUnitTest
{
    [TestClass]
    public class Md5Test
    {
        [TestMethod]
        public void GetMD5Hash_Base64()
        {
            var hash = MD5.GetMD5Hash("123456", base64: true);
            Assert.IsFalse(string.IsNullOrEmpty(hash));
            // 123456 的 MD5 Base64 已知值
            Assert.AreEqual("4QrcOUm6Wau+VuBX8g+IPg==", hash);
        }

        [TestMethod]
        public void GetMD5Hash_Hex()
        {
            var hash = MD5.GetMD5Hash("123456", base64: false);
            Assert.AreEqual("e10adc3949ba59abbe56e057f20f883e", hash);
        }

        [TestMethod]
        public void VerifyMD5Hash_Base64()
        {
            var hash = MD5.GetMD5Hash("hello", base64: true);
            Assert.IsTrue(MD5.VerifyMD5Hash("hello", hash, base64: true));
            Assert.IsFalse(MD5.VerifyMD5Hash("hello", hash + "x", base64: true));
        }

        [TestMethod]
        public void VerifyMD5Hash_Hex_CaseInsensitive()
        {
            var hash = MD5.GetMD5Hash("test", base64: false);
            Assert.IsTrue(MD5.VerifyMD5Hash("test", hash.ToUpperInvariant(), base64: false));
        }

        [TestMethod]
        public void GetMD5Hash_EmptyString()
        {
            var hash = MD5.GetMD5Hash("", base64: false);
            Assert.AreEqual(32, hash.Length);
        }
    }
}
