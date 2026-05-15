namespace Utils;

public static class CRC
{
    private static byte[] ComputeCrc16(byte[] data, int len, int xdapoly, bool crc16 = true)
    {
        if (data is null) throw new ArgumentNullException(nameof(data));
        if (len < 0 || len > data.Length) throw new ArgumentOutOfRangeException(nameof(len));

        int xda = 0xFFFF;
        for (int i = 0; i < len; i++)
        {
            xda = (xda >> (crc16 ? 0 : 8)) ^ data[i];
            for (int j = 0; j < 8; j++)
            {
                int xdabit = xda & 0x01;
                xda >>= 1;
                if (xdabit == 1)
                    xda ^= xdapoly;
            }
        }
        return crc16 ? [(byte)(xda & 0xFF), (byte)(xda >> 8)] : [(byte)(xda >> 8), (byte)(xda & 0xFF)];
    }

    public static byte[] Crc16(byte[] data, int len)
    {
        return ComputeCrc16(data, len, 0xA001);
    }

    public static byte[] GBcrc16(byte[] data, int len)
    {
        return ComputeCrc16(data, len, 0xA001, false);
    }

    public static byte[] HBcrc16(byte[] data, int len)
    {
        return ComputeCrc16(data, len, 0xFAC1, false);
    }

    public static byte[] Crc16(byte[] data)
    {
        if (data is null) throw new ArgumentNullException(nameof(data));
        return [.. data, .. ComputeCrc16(data, data.Length, 0xA001)];
    }

    public static byte[] CRC16_R(byte[] data)
    {
        if (data is null) throw new ArgumentNullException(nameof(data));
        var crc = Crc16(data, data.Length);
        Array.Reverse(crc);
        return crc;
    }

    public static ushort UpdateCRC(byte[] input, int length)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input));
        if (length < 0 || length > input.Length)
            throw new ArgumentOutOfRangeException(nameof(length));

        ushort crc = 0xFFFF;
        for (int j = 0; j < length; j++)
        {
            crc ^= (ushort)(input[j] << 8);
            for (int i = 0; i < 8; i++)
            {
                if ((crc & 0x8000) != 0)
                {
                    crc = (ushort)((crc << 1) ^ 0x1021);
                }
                else
                {
                    crc <<= 1;
                }
            }
        }
        return crc;
    }

    public static byte[] UpdateCRC(byte[] input)
    {
        return StringByteUtils.GetBytes(UpdateCRC(input, input.Length), true);
    }
}
