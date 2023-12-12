namespace Utils;

public static class CRC
{
    public static byte[] Crc16(byte[] data, int len)
    {
        byte[] temdata = new byte[2];
        int xda, xdapoly;
        int i, j, xdabit;
        xda = 0xFFFF;
        xdapoly = 0xA001;
        for (i = 0; i < len; i++)
        {
            xda ^= data[i];
            for (j = 0; j < 8; j++)
            {
                xdabit = (int)(xda & 0x01);
                xda >>= 1;
                if (xdabit == 1)
                    xda ^= xdapoly;
            }
        }
        temdata[0] = (byte)(xda & 0xFF);
        temdata[1] = (byte)(xda >> 8);
        return temdata;
    }

    public static byte[] GBcrc16(byte[] data, int len)
    {
        byte[] temdata = new byte[2];
        int xda, xdapoly;
        int i, j, xdabit;
        xda = 0xFFFF;
        xdapoly = 0xA001;
        for (i = 0; i < len; i++)
        {
            xda = (xda >> 8) ^ data[i];
            for (j = 0; j < 8; j++)
            {
                xdabit = (int)(xda & 0x01);
                xda >>= 1;
                if (xdabit == 1)
                    xda ^= xdapoly;
            }
        }
        temdata[0] = (byte)(xda >> 8);
        temdata[1] = (byte)(xda & 0xFF);
        return temdata;
    }

    public static byte[] HBcrc16(byte[] data, int len)
    {
        byte[] temdata = new byte[2];
        int xda, xdapoly;
        int i, j, xdabit;
        xda = 0xFFFF;
        xdapoly = 0xFAC1;
        for (i = 0; i < len; i++)
        {
            xda = (xda >> 8) ^ data[i];
            for (j = 0; j < 8; j++)
            {
                xdabit = (int)(xda & 0x01);
                xda >>= 1;
                if (xdabit == 1)
                    xda ^= xdapoly;
            }
        }
        temdata[0] = (byte)(xda >> 8);
        temdata[1] = (byte)(xda & 0xFF);
        return temdata;
    }

    public static byte[] Crc16(byte[] data)
    {
        byte[] temdata = new byte[data.Length + 2];
        int xda, xdapoly;
        int i, j, xdabit;
        xda = 0xFFFF;
        xdapoly = 0xA001;
        for (i = 0; i < data.Length; i++)
        {
            xda ^= data[i];
            for (j = 0; j < 8; j++)
            {
                xdabit = (int)(xda & 0x01);
                xda >>= 1;
                if (xdabit == 1)
                    xda ^= xdapoly;
            }
        }
        Array.Copy(data, 0, temdata, 0, data.Length);
        temdata[temdata.Length - 2] = (byte)(xda & 0xFF);
        temdata[temdata.Length - 1] = (byte)(xda >> 8);
        return temdata;
    }

    public static byte[] CRC16_C(byte[] data)
    {
        byte CRC16Lo;
        byte CRC16Hi;   //CRC寄存器 
        byte CL; byte CH;       //多项式码&HA001 
        byte SaveHi; byte SaveLo;
        byte[] tmpData;
        //int I;
        int Flag;
        CRC16Lo = 0xFF;
        CRC16Hi = 0xFF;
        CL = 0x01;
        CH = 0xA0;
        tmpData = data;
        for (int i = 0; i < tmpData.Length; i++)
        {
            CRC16Lo = (byte)(CRC16Lo ^ tmpData[i]); //每一个数据与CRC寄存器进行异或 
            for (Flag = 0; Flag <= 7; Flag++)
            {
                SaveHi = CRC16Hi;
                SaveLo = CRC16Lo;
                CRC16Hi = (byte)(CRC16Hi >> 1);      //高位右移一位 
                CRC16Lo = (byte)(CRC16Lo >> 1);      //低位右移一位 
                if ((SaveHi & 0x01) == 0x01) //如果高位字节最后一位为1 
                {
                    CRC16Lo = (byte)(CRC16Lo | 0x80);   //则低位字节右移后前面补1 
                }             //否则自动补0 
                if ((SaveLo & 0x01) == 0x01) //如果LSB为1，则与多项式码进行异或 
                {
                    CRC16Hi = (byte)(CRC16Hi ^ CH);
                    CRC16Lo = (byte)(CRC16Lo ^ CL);
                }
            }
        }
        byte[] ReturnData =
        [
            CRC16Hi,       //CRC高位 
            CRC16Lo,       //CRC低位 
        ];
        return ReturnData;
    }

    public static ushort UpdateCRC(byte[] input, int Length)
    {

        short j;
        short i; // loop counter
        ushort CRC_acc = 0xFFFF;
        for (j = 0; j < Length; j++)
        {
            // Create the CRC "dividend" for polynomial arithmetic (binary arithmetic
            // with no carries)
            CRC_acc = (ushort)(CRC_acc ^ (input[j] << 8));
            // "Divide" the poly into the dividend using CRC XOR subtraction
            // CRC_acc holds the "remainder" of each divide
            //
            // Only complete this division for 8 bits since input is 1 byte
            for (i = 0; i < 8; i++)
            {
                // Check if the MSB is set (if MSB is 1, then the POLY can "divide"
                // into the "dividend")
                if ((CRC_acc & 0x8000) == 0x8000)
                {
                    // if so, shift the CRC value, and XOR "subtract" the poly
                    CRC_acc = (ushort)(CRC_acc << 1);
                    CRC_acc ^= 0x1021;
                }
                else
                {
                    // if not, just shift the CRC value
                    CRC_acc = (ushort)(CRC_acc << 1);
                }
            }

        }
        // Return the final remainder (CRC value)
        return CRC_acc;
    }
}
