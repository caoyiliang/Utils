namespace Utils;

public static class CRC
{
    private static byte[] ComputeCrc16(byte[] data, int len, int xdapoly, bool Crc16 = true)
    {
        int xda, i, j, xdabit;
        xda = 0xFFFF;
        for (i = 0; i < len; i++)
        {
            xda = (xda >> (Crc16 ? 0 : 8)) ^ data[i];
            for (j = 0; j < 8; j++)
            {
                xdabit = xda & 0x01;
                xda >>= 1;
                if (xdabit == 1)
                    xda ^= xdapoly;
            }
        }
        return Crc16 ? [(byte)(xda & 0xFF), (byte)(xda >> 8)] : [(byte)(xda >> 8), (byte)(xda & 0xFF)];
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
        return [.. data, .. ComputeCrc16(data, data.Length, 0xA001)];
    }

    public static byte[] CRC16_R(byte[] data)
    {
        var crc = Crc16(data, data.Length);
        Array.Reverse(crc);
        return crc;
    }

    public static ushort UpdateCRC(byte[] input, int length)
    {
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
