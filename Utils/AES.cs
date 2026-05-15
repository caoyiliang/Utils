using System.Security.Cryptography;
using System.Text;

namespace Utils;

public class AES : IDisposable
{
    private readonly Aes _aes;

    /// <summary>
    /// 创建 AES 实例
    /// </summary>
    /// <param name="key">密钥</param>
    /// <param name="mode">加密模式，建议使用 CBC</param>
    /// <param name="padding">填充模式</param>
    /// <param name="iv">初始化向量，非 ECB 模式时必须提供</param>
    public AES(byte[] key, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7, byte[]? iv = null)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));
        if (key.Length != 16 && key.Length != 24 && key.Length != 32)
            throw new ArgumentException("密钥长度必须为 16、24 或 32 字节", nameof(key));

        _aes = Aes.Create();
        _aes.Key = key;
        _aes.Mode = mode;
        _aes.Padding = padding;

        if (mode != CipherMode.ECB)
        {
            _aes.IV = iv ?? throw new ArgumentNullException(nameof(iv), $"{mode} 模式需要提供 IV");
        }
    }

    /// <summary>
    /// AES 加密
    /// </summary>
    /// <param name="data">明文（待加密）</param>
    public string Encrypt(string data)
    {
        if (string.IsNullOrEmpty(data)) throw new ArgumentNullException(nameof(data));

        using var encryptor = _aes.CreateEncryptor();
        var dataArray = Encoding.UTF8.GetBytes(data);
        var rs = encryptor.TransformFinalBlock(dataArray, 0, dataArray.Length);
        return Convert.ToBase64String(rs);
    }

    /// <summary>
    /// AES 解密
    /// </summary>
    /// <param name="data">密文（待解密）</param>
    public string Decrypt(string data)
    {
        if (string.IsNullOrEmpty(data)) throw new ArgumentNullException(nameof(data));

        using var decryptor = _aes.CreateDecryptor();
        var base64Data = Convert.FromBase64String(data);
        var rs = decryptor.TransformFinalBlock(base64Data, 0, base64Data.Length);
        return Encoding.UTF8.GetString(rs);
    }

    public void Dispose()
    {
        _aes?.Dispose();
        GC.SuppressFinalize(this);
    }
}
