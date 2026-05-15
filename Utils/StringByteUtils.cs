using System.Text;

namespace Utils;

public static class StringByteUtils
{
    public static string BytesToString(byte[] data)
    {
        if (data is null) throw new ArgumentNullException(nameof(data));
        return BytesToString(data, data.Length);
    }

    public static string BytesToString(byte[] data, int count)
    {
        if (data == null || data.Length == 0 || count <= 0)
        {
            return string.Empty;
        }
        if (count > data.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }
        var sb = new StringBuilder(count * 3 - 1);
        for (int i = 0; i < count; i++)
        {
            sb.AppendHexByte(data[i]);
            if (i != count - 1) sb.Append(' ');
        }
        return sb.ToString();
    }

    public static string ByteToString(byte value) => value.ToString("X2");

    private static void AppendHexByte(this StringBuilder sb, byte value)
    {
        sb.Append(HexChars[value >> 4]);
        sb.Append(HexChars[value & 0xF]);
    }

    private static readonly char[] HexChars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];

    public static byte[] StringToBytes(string s)
    {
        if (s is null) throw new ArgumentNullException(nameof(s));
        s = s.Replace("0x", "").Replace(" ", "");
        if (s.Length % 2 != 0)
            throw new ArgumentException("字符串长度必须为偶数", nameof(s));
        byte[] buffer = new byte[s.Length / 2];
        for (int i = 0; i < s.Length; i += 2)
        {
            buffer[i / 2] = (byte)((HexValue(s[i]) << 4) | HexValue(s[i + 1]));
        }
        return buffer;
    }

    private static int HexValue(char c)
    {
        if (c >= '0' && c <= '9') return c - '0';
        if (c >= 'A' && c <= 'F') return c - 'A' + 10;
        if (c >= 'a' && c <= 'f') return c - 'a' + 10;
        throw new ArgumentException($"非法的十六进制字符: '{c}'");
    }

    /// <summary>合并任意个byte数组</summary>
    [Obsolete("请使用 CombineByteArray")]
    public static byte[] ComibeByteArray(params byte[][] bytes) => CombineByteArray(bytes);

    /// <summary>合并任意个byte数组</summary>
    [Obsolete("请使用 CombineByteArray")]
    public static byte[] ComibeByteArray(IEnumerable<byte[]> bytes) => CombineByteArray(bytes);

    /// <summary>合并任意个byte数组</summary>
    public static byte[] CombineByteArray(params byte[][] bytes)
    {
        if (bytes is null) throw new ArgumentNullException(nameof(bytes));
        int totalLength = 0;
        foreach (byte[] item in bytes)
        {
            if (item is null) throw new ArgumentNullException(nameof(bytes), "数组中包含 null 元素");
            totalLength += item.Length;
        }
        byte[] rs = new byte[totalLength];
        int offset = 0;
        foreach (byte[] item in bytes)
        {
            item.CopyTo(rs, offset);
            offset += item.Length;
        }
        return rs;
    }

    /// <summary>合并任意个byte数组</summary>
    public static byte[] CombineByteArray(IEnumerable<byte[]> bytes)
    {
        if (bytes is null) throw new ArgumentNullException(nameof(bytes));
        if (bytes is ICollection<byte[]> collection)
        {
            int totalLength = 0;
            foreach (byte[] item in collection)
            {
                totalLength += item.Length;
            }
            byte[] rs = new byte[totalLength];
            int offset = 0;
            foreach (byte[] item in collection)
            {
                item.CopyTo(rs, offset);
                offset += item.Length;
            }
            return rs;
        }
        else
        {
            return CombineByteArray(bytes.ToArray());
        }
    }

    public static bool ValueEqual(this byte[]? array1, byte[]? array2)
    {
        if (array1 == null && array2 == null)
            return true;

        if (array1 == null || array2 == null)
            return false;

        return array1.AsSpan().SequenceEqual(array2);
    }

    public static double ToDouble(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        return ToDouble(value.AsSpan(), startIndex, isHighByteBefore);
    }

    public static double ToDouble(ReadOnlySpan<byte> value, int startIndex, bool isHighByteBefore = false)
    {
        var span = value.Slice(startIndex, 8);
#if NET8_0_OR_GREATER
        return isHighByteBefore
            ? System.Buffers.Binary.BinaryPrimitives.ReadDoubleBigEndian(span)
            : System.Buffers.Binary.BinaryPrimitives.ReadDoubleLittleEndian(span);
#else
        byte[] temp = new byte[8];
        span.CopyTo(temp);
        if (isHighByteBefore)
            Array.Reverse(temp);
        return BitConverter.ToDouble(temp, 0);
#endif
    }

    public static short ToInt16(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        return ToInt16(value.AsSpan(), startIndex, isHighByteBefore);
    }

    public static short ToInt16(ReadOnlySpan<byte> value, int startIndex, bool isHighByteBefore = false)
    {
        var span = value.Slice(startIndex, 2);
#if NET8_0_OR_GREATER
        return isHighByteBefore
            ? System.Buffers.Binary.BinaryPrimitives.ReadInt16BigEndian(span)
            : System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(span);
#else
        byte[] temp = new byte[2];
        span.CopyTo(temp);
        if (isHighByteBefore)
            Array.Reverse(temp);
        return BitConverter.ToInt16(temp, 0);
#endif
    }

    public static int ToInt32(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        return ToInt32(value.AsSpan(), startIndex, isHighByteBefore);
    }

    public static int ToInt32(ReadOnlySpan<byte> value, int startIndex, bool isHighByteBefore = false)
    {
        var span = value.Slice(startIndex, 4);
#if NET8_0_OR_GREATER
        return isHighByteBefore
            ? System.Buffers.Binary.BinaryPrimitives.ReadInt32BigEndian(span)
            : System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
#else
        byte[] temp = new byte[4];
        span.CopyTo(temp);
        if (isHighByteBefore)
            Array.Reverse(temp);
        return BitConverter.ToInt32(temp, 0);
#endif
    }

    public static long ToInt64(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        return ToInt64(value.AsSpan(), startIndex, isHighByteBefore);
    }

    public static long ToInt64(ReadOnlySpan<byte> value, int startIndex, bool isHighByteBefore = false)
    {
        var span = value.Slice(startIndex, 8);
#if NET8_0_OR_GREATER
        return isHighByteBefore
            ? System.Buffers.Binary.BinaryPrimitives.ReadInt64BigEndian(span)
            : System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(span);
#else
        byte[] temp = new byte[8];
        span.CopyTo(temp);
        if (isHighByteBefore)
            Array.Reverse(temp);
        return BitConverter.ToInt64(temp, 0);
#endif
    }

    public static float ToSingle(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        return ToSingle(value.AsSpan(), startIndex, isHighByteBefore);
    }

    public static float ToSingle(ReadOnlySpan<byte> value, int startIndex, bool isHighByteBefore = false)
    {
        var span = value.Slice(startIndex, 4);
#if NET8_0_OR_GREATER
        return isHighByteBefore
            ? System.Buffers.Binary.BinaryPrimitives.ReadSingleBigEndian(span)
            : System.Buffers.Binary.BinaryPrimitives.ReadSingleLittleEndian(span);
#else
        byte[] temp = new byte[4];
        span.CopyTo(temp);
        if (isHighByteBefore)
            Array.Reverse(temp);
        return BitConverter.ToSingle(temp, 0);
#endif
    }

    public static string ToString(byte[] value, bool isHighByteBefore = false)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        var temp = value;
        if (isHighByteBefore)
        {
            temp = (byte[])value.Clone();
            Array.Reverse(temp);
        }
        return BitConverter.ToString(temp);
    }

    public static string ToString(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        var temp = value;
        if (isHighByteBefore)
        {
            temp = (byte[])value.Clone();
            Array.Reverse(temp, startIndex, value.Length - startIndex);
        }
        return BitConverter.ToString(temp, startIndex);
    }

    public static string ToString(byte[] value, int startIndex, int length, bool isHighByteBefore = false)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        var temp = value;
        if (isHighByteBefore)
        {
            temp = (byte[])value.Clone();
            Array.Reverse(temp, startIndex, length);
        }
        return BitConverter.ToString(temp, startIndex, length);
    }

    public static ushort ToUInt16(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        return ToUInt16(value.AsSpan(), startIndex, isHighByteBefore);
    }

    public static ushort ToUInt16(ReadOnlySpan<byte> value, int startIndex, bool isHighByteBefore = false)
    {
        var span = value.Slice(startIndex, 2);
#if NET8_0_OR_GREATER
        return isHighByteBefore
            ? System.Buffers.Binary.BinaryPrimitives.ReadUInt16BigEndian(span)
            : System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(span);
#else
        byte[] temp = new byte[2];
        span.CopyTo(temp);
        if (isHighByteBefore)
            Array.Reverse(temp);
        return BitConverter.ToUInt16(temp, 0);
#endif
    }

    public static uint ToUInt32(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        return ToUInt32(value.AsSpan(), startIndex, isHighByteBefore);
    }

    public static uint ToUInt32(ReadOnlySpan<byte> value, int startIndex, bool isHighByteBefore = false)
    {
        var span = value.Slice(startIndex, 4);
#if NET8_0_OR_GREATER
        return isHighByteBefore
            ? System.Buffers.Binary.BinaryPrimitives.ReadUInt32BigEndian(span)
            : System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(span);
#else
        byte[] temp = new byte[4];
        span.CopyTo(temp);
        if (isHighByteBefore)
            Array.Reverse(temp);
        return BitConverter.ToUInt32(temp, 0);
#endif
    }

    public static ulong ToUInt64(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));
        return ToUInt64(value.AsSpan(), startIndex, isHighByteBefore);
    }

    public static ulong ToUInt64(ReadOnlySpan<byte> value, int startIndex, bool isHighByteBefore = false)
    {
        var span = value.Slice(startIndex, 8);
#if NET8_0_OR_GREATER
        return isHighByteBefore
            ? System.Buffers.Binary.BinaryPrimitives.ReadUInt64BigEndian(span)
            : System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span);
#else
        byte[] temp = new byte[8];
        span.CopyTo(temp);
        if (isHighByteBefore)
            Array.Reverse(temp);
        return BitConverter.ToUInt64(temp, 0);
#endif
    }

    public static byte[] GetBytes(short value, bool isHighByteBefore = false)
    {
#if NET5_0_OR_GREATER
        byte[] arr = new byte[sizeof(short)];
        if (isHighByteBefore)
            System.Buffers.Binary.BinaryPrimitives.WriteInt16BigEndian(arr, value);
        else
            System.Buffers.Binary.BinaryPrimitives.WriteInt16LittleEndian(arr, value);
        return arr;
#else
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
#endif
    }

    public static byte[] GetBytes(int value, bool isHighByteBefore = false)
    {
#if NET5_0_OR_GREATER
        byte[] arr = new byte[sizeof(int)];
        if (isHighByteBefore)
            System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(arr, value);
        else
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(arr, value);
        return arr;
#else
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
#endif
    }

    public static byte[] GetBytes(long value, bool isHighByteBefore = false)
    {
#if NET5_0_OR_GREATER
        byte[] arr = new byte[sizeof(long)];
        if (isHighByteBefore)
            System.Buffers.Binary.BinaryPrimitives.WriteInt64BigEndian(arr, value);
        else
            System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(arr, value);
        return arr;
#else
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
#endif
    }

    public static byte[] GetBytes(ushort value, bool isHighByteBefore = false)
    {
#if NET5_0_OR_GREATER
        byte[] arr = new byte[sizeof(ushort)];
        if (isHighByteBefore)
            System.Buffers.Binary.BinaryPrimitives.WriteUInt16BigEndian(arr, value);
        else
            System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(arr, value);
        return arr;
#else
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
#endif
    }

    public static byte[] GetBytes(uint value, bool isHighByteBefore = false)
    {
#if NET5_0_OR_GREATER
        byte[] arr = new byte[sizeof(uint)];
        if (isHighByteBefore)
            System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(arr, value);
        else
            System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(arr, value);
        return arr;
#else
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
#endif
    }

    public static byte[] GetBytes(ulong value, bool isHighByteBefore = false)
    {
#if NET5_0_OR_GREATER
        byte[] arr = new byte[sizeof(ulong)];
        if (isHighByteBefore)
            System.Buffers.Binary.BinaryPrimitives.WriteUInt64BigEndian(arr, value);
        else
            System.Buffers.Binary.BinaryPrimitives.WriteUInt64LittleEndian(arr, value);
        return arr;
#else
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
#endif
    }

    public static byte[] GetBytes(float value, bool isHighByteBefore = false)
    {
#if NET5_0_OR_GREATER
        byte[] arr = new byte[sizeof(float)];
        if (isHighByteBefore)
            System.Buffers.Binary.BinaryPrimitives.WriteSingleBigEndian(arr, value);
        else
            System.Buffers.Binary.BinaryPrimitives.WriteSingleLittleEndian(arr, value);
        return arr;
#else
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
#endif
    }

    public static byte[] GetBytes(double value, bool isHighByteBefore = false)
    {
#if NET5_0_OR_GREATER
        byte[] arr = new byte[sizeof(double)];
        if (isHighByteBefore)
            System.Buffers.Binary.BinaryPrimitives.WriteDoubleBigEndian(arr, value);
        else
            System.Buffers.Binary.BinaryPrimitives.WriteDoubleLittleEndian(arr, value);
        return arr;
#else
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
#endif
    }
}
