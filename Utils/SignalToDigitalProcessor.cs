namespace Utils;

/// <param name="minSignalValue">电流/电压 最小值</param>
/// <param name="maxSignalValue">电流/电压 最大值</param>
/// <param name="minDigitalValue">对应数值 最小值</param>
/// <param name="maxDigitalValue">对应数值 最大值</param>
public class SignalToDigitalProcessor(decimal minSignalValue, decimal maxSignalValue, decimal minDigitalValue, decimal maxDigitalValue)
{
    public decimal InputDataProcess(decimal inputValue)
    {
        return (maxDigitalValue - minDigitalValue)
               / (maxSignalValue - minSignalValue)
               * (inputValue - minSignalValue)
               + minDigitalValue;
    }

    public decimal OutputDataProcess(decimal outputValue)
    {
        if (outputValue <= minDigitalValue)
        {
            return minSignalValue;
        }
        else if (outputValue >= maxDigitalValue)
        {
            return maxSignalValue;
        }
        else
        {
            return (maxSignalValue - minSignalValue)
                   / (maxDigitalValue - minDigitalValue)
                   * (outputValue - minDigitalValue)
                   + minSignalValue;
        }
    }
}
