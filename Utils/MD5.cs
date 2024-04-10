using System.Text;

namespace Utils;

/// <summary>
/// MD5加密类
/// </summary>
public class MD5
{
    public static string GetMD5Hash(string str, bool base64 = true)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
        if (base64) return Convert.ToBase64String(data);
        else
        {
            var sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// 验证
    /// </summary>
    public static bool VerifyMD5Hash(string str, string hash, bool base64 = true)
    {
        string hashOfInput = GetMD5Hash(str, base64);
        return hashOfInput.Equals(hash, StringComparison.OrdinalIgnoreCase);
    }
}