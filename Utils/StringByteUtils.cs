﻿using System.Text;

namespace Utils;

public static class StringByteUtils
{
    public static string BytesToString(byte[] data)
    {
        return BytesToString(data, data.Length);
    }

    public static string BytesToString(byte[] data, int count)
    {
        if (data == null || data.Length == 0)
        {
            return string.Empty;
        }
        var sb = new StringBuilder();
        for (int i = 0; i < count; i++)
        {
            string s = ByteToString(data[i]);
            sb.Append(s);
            if (i != count - 1) sb.Append(' ');
        }
        return sb.ToString();
    }

    public static string ByteToString(byte value)
    {
        return value.ToString("X2");
    }

    public static byte[] StringToBytes(string s)
    {
        s = s.Replace(" ", "");
        byte[] buffer = new byte[s.Length / 2];
        for (int i = 0; i < s.Length; i += 2)
            buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
        return buffer;
    }

    /// <summary>合并任意个byte数组</summary>
    public static byte[] ComibeByteArray(params byte[][] bytes)
    {
        byte[] rs = [];
        foreach (byte[] item in bytes)
        {
            byte[] temp = rs;
            int len = rs.Length;
            rs = new byte[len + item.Length];
            temp.CopyTo(rs, 0);
            item.CopyTo(rs, len);
        }
        return rs;
    }

    /// <summary>合并任意个byte数组</summary>
    public static byte[] ComibeByteArray(IEnumerable<byte[]> bytes)
    {
        byte[] rs = [];
        foreach (byte[] item in bytes)
        {
            byte[] temp = rs;
            int len = rs.Length;
            rs = new byte[len + item.Length];
            temp.CopyTo(rs, 0);
            item.CopyTo(rs, len);
        }
        return rs;
    }

    public static double ToDouble(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        var temp = value;
        if (isHighByteBefore)
        {
            temp = (byte[])value.Clone();
            Array.Reverse(temp, startIndex, 8);
        }
        return BitConverter.ToDouble(temp, startIndex);
    }

    public static short ToInt16(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        var temp = value;
        if (isHighByteBefore)
        {
            temp = (byte[])value.Clone();
            Array.Reverse(temp, startIndex, 2);
        }
        return BitConverter.ToInt16(temp, startIndex);
    }

    public static int ToInt32(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        var temp = value;
        if (isHighByteBefore)
        {
            temp = (byte[])value.Clone();
            Array.Reverse(temp, startIndex, 4);
        }
        return BitConverter.ToInt32(temp, startIndex);
    }

    public static long ToInt64(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        var temp = value;
        if (isHighByteBefore)
        {
            temp = (byte[])value.Clone();
            Array.Reverse(temp, startIndex, 8);
        }
        return BitConverter.ToInt64(temp, startIndex);
    }

    public static float ToSingle(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        var temp = value;
        if (isHighByteBefore)
        {
            temp = (byte[])value.Clone();
            Array.Reverse(temp, startIndex, 4);
        }
        return BitConverter.ToSingle(temp, startIndex);
    }

    public static string ToString(byte[] value, bool isHighByteBefore = false)
    {
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
        var temp = value;
        if (isHighByteBefore)
        {
            temp = (byte[])value.Clone();
            Array.Reverse(temp, startIndex, value.Length - (startIndex + 1));
        }
        return BitConverter.ToString(temp, startIndex);
    }

    public static string ToString(byte[] value, int startIndex, int length, bool isHighByteBefore = false)
    {
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
        var temp = value;
        if (isHighByteBefore)
        {
            temp = (byte[])value.Clone();
            Array.Reverse(temp, startIndex, 2);
        }
        return BitConverter.ToUInt16(temp, startIndex);
    }

    public static uint ToUInt32(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        var temp = value;
        if (isHighByteBefore)
        {
            temp = (byte[])value.Clone();
            Array.Reverse(temp, startIndex, 4);
        }
        return BitConverter.ToUInt32(temp, startIndex);
    }

    public static ulong ToUInt64(byte[] value, int startIndex, bool isHighByteBefore = false)
    {
        var temp = value;
        if (isHighByteBefore)
        {
            temp = (byte[])value.Clone();
            Array.Reverse(temp, startIndex, 8);
        }
        return BitConverter.ToUInt64(temp, startIndex);
    }

    public static byte[] GetBytes(short value, bool isHighByteBefore = false)
    {
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
    }

    public static byte[] GetBytes(int value, bool isHighByteBefore = false)
    {
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
    }
    public static byte[] GetBytes(long value, bool isHighByteBefore = false)
    {
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
    }

    public static byte[] GetBytes(ushort value, bool isHighByteBefore = false)
    {
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
    }

    public static byte[] GetBytes(uint value, bool isHighByteBefore = false)
    {
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
    }

    public static byte[] GetBytes(ulong value, bool isHighByteBefore = false)
    {
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
    }

    public static byte[] GetBytes(float value, bool isHighByteBefore = false)
    {
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
    }

    public static byte[] GetBytes(double value, bool isHighByteBefore = false)
    {
        if (isHighByteBefore)
        {
            var arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
            return arr;
        }
        return BitConverter.GetBytes(value);
    }
}
