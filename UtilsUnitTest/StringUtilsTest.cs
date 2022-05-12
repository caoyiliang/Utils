using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
