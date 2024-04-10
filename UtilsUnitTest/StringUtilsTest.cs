using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text;
using Utils;

namespace UtilsUnitTest
{
    [TestClass]
    public class StringUtilsTest
    {
        [TestMethod]
        public async Task TestGetAllNum()
        {
            var str = @" 
>-3.2441-3.1245+3.2423+3.1817+3.1340+2.5312+2.2312-2.5312";
            var result = str.GetAllNum();
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
            await Task.CompletedTask;
        }

        [TestMethod]
        public async Task TestAes()
        {
            using var aes = new AES(Encoding.UTF8.GetBytes("sciencetechnolog"));
            var a = aes.Encrypt("adfasdf");
            var b = aes.Decrypt(a);
            await Task.CompletedTask;
        }

        [TestMethod]
        public async Task TestReTry()
        {
            var func = () => TaskA();
            await func.ReTry(3);
        }

        private Task TaskA()
        {
            Debug.Write("A");
            throw new Exception();
        }

        [TestMethod]
        public async Task TestMD5()
        {
            var str = "123456";
            var md5 = MD5.GetMD5Hash(str);
            var result = MD5.VerifyMD5Hash(str, md5);
            Assert.IsTrue(result);
            await Task.CompletedTask;
        }

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
            await Task.CompletedTask;
        }

        [TestMethod]
        public async Task TestComibeByteArray()
        {
            byte[] a = [0x01, 0x02, 0x03];
            byte[] b = [0x04, 0x05, 0x06];
            var result = StringByteUtils.ComibeByteArray(a, b);
            await Task.CompletedTask;
        }
    }
}
