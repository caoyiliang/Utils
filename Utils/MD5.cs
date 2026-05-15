using System.Text;

namespace Utils;

/// <summary>MD5 工具类</summary>
public class MD5
{
    public static string GetMD5Hash(string str, bool base64 = true)
    {
        if (str is null) throw new ArgumentNullException(nameof(str));
#if NET5_0_OR_GREATER
        byte[] data = System.Security.Cryptography.MD5.HashData(Encoding.UTF8.GetBytes(str));
        return base64
            ? Convert.ToBase64String(data)
            : Convert.ToHexString(data).ToLowerInvariant();
#else
        using var md5 = System.Security.Cryptography.MD5.Create();
        byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
        if (base64) return Convert.ToBase64String(data);
        var sb = new StringBuilder(32);
        for (int i = 0; i < data.Length; i++)
        {
            sb.Append(data[i].ToString("x2"));
        }
        return sb.ToString();
#endif
    }

    /// <summary>验证 MD5 哈希</summary>
    public static bool VerifyMD5Hash(string str, string hash, bool base64 = true)
    {
        if (str is null) throw new ArgumentNullException(nameof(str));
        if (hash is null) throw new ArgumentNullException(nameof(hash));
        string hashOfInput = GetMD5Hash(str, base64);
        return hashOfInput.Equals(hash, StringComparison.OrdinalIgnoreCase);
    }
}
