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
    }
}
