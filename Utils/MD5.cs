using System;
using System.Security.Cryptography;
using System.Text;

namespace Utils
{
    /// <summary>
    /// MD5加密类
    /// </summary>
    public class MD5
    {
        public static string GetMD5Hash(string str, bool base64 = true)
        {
            //就是比string往后一直加要好的优化容器
            StringBuilder sb = new StringBuilder();
            using MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //将输入字符串转换为字节数组并计算哈希。
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            if (base64) return Convert.ToBase64String(data);
            else
            {
                //X为     十六进制 X都是大写 x都为小写
                //2为 每次都是两位数
                //假设有两个数10和26，正常情况十六进制显示0xA、0x1A，这样看起来不整齐，为了好看，可以指定"X2"，这样显示出来就是：0x0A、0x1A。 
                //遍历哈希数据的每个字节
                //并将每个字符串格式化为十六进制字符串。
                int length = data.Length;
                for (int i = 0; i < length; i++)
                    sb.Append(data[i].ToString("X2"));
                return sb.ToString();
            }
        }

        //验证
        public static bool VerifyMD5Hash(string str, string hash, bool base64 = true)
        {
            string hashOfInput = GetMD5Hash(str, base64);
            return hashOfInput.CompareTo(hash) == 0;
        }
    }
}