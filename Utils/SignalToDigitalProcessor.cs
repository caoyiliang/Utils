namespace Utils;

/// <param name="minSignalValue">电流/电压 最小值</param>
/// <param name="maxSignalValue">电流/电压 最大值</param>
/// <param name="minDigitalValue">对应数值 最小值</param>
/// <param name="maxDigitalValue">对应数值 最大值</param>
public class SignalToDigitalProcessor(decimal minSignalValue, decimal maxSignalValue, decimal minDigitalValue, decimal maxDigitalValue)
{
    private readonly decimal _inputScaleFactor = (maxDigitalValue - minDigitalValue) / (maxSignalValue - minSignalValue);
    private readonly decimal _outputScaleFactor = (maxSignalValue - minSignalValue) / (maxDigitalValue - minDigitalValue);

    public decimal InputDataProcess(decimal inputValue)
    {
        return _inputScaleFactor * (inputValue - minSignalValue) + minDigitalValue;
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
            return _outputScaleFactor * (outputValue - minDigitalValue) + minSignalValue;
        }
    }
}
