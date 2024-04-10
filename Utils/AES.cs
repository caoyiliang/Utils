using System.Security.Cryptography;
using System.Text;

namespace Utils;

public class AES : IDisposable
{
    private readonly Aes _aes;
    private readonly ICryptoTransform _encryptor;
    private readonly ICryptoTransform _decryptor;
    public AES(byte[] key, CipherMode mode = CipherMode.ECB, PaddingMode padding = PaddingMode.PKCS7)
    {
        _aes = Aes.Create();
        _aes.Key = key;
        _aes.Mode = mode;
        _aes.Padding = padding;
        _encryptor = _aes.CreateEncryptor();
        _decryptor = _aes.CreateDecryptor();
    }

    /// <summary>
    ///  AES 加密
    /// </summary>
    /// <param name="data">明文（待加密）</param>
    public string Encrypt(string data)
    {
        if (string.IsNullOrEmpty(data)) throw new ArgumentNullException(nameof(data));

        var dataArray = Encoding.UTF8.GetBytes(data);
        var rs = _encryptor.TransformFinalBlock(dataArray, 0, dataArray.Length);
        return Convert.ToBase64String(rs);
    }

    /// <summary>
    ///  AES 解密
    /// </summary>
    /// <param name="data">明文（待解密）</param>
    /// <returns></returns>
    public string Decrypt(string data)
    {
        if (string.IsNullOrEmpty(data)) throw new ArgumentNullException(nameof(data));

        var base64Data = Convert.FromBase64String(data);
        var rs = _decryptor.TransformFinalBlock(base64Data, 0, base64Data.Length);
        return Encoding.UTF8.GetString(rs);
    }

    public void Dispose()
    {
        _encryptor?.Dispose();
        _decryptor?.Dispose();
        _aes?.Dispose();
        GC.SuppressFinalize(this);
    }
}
