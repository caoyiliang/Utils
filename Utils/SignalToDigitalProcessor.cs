namespace Utils;

/// <summary>信号与数字量转换器</summary>
public class SignalToDigitalProcessor
{
    private readonly decimal _minSignalValue;
    private readonly decimal _maxSignalValue;
    private readonly decimal _minDigitalValue;
    private readonly decimal _maxDigitalValue;
    private readonly decimal _inputScaleFactor;
    private readonly decimal _outputScaleFactor;

    /// <param name="minSignalValue">电流/电压 最小值</param>
    /// <param name="maxSignalValue">电流/电压 最大值</param>
    /// <param name="minDigitalValue">对应数值 最小值</param>
    /// <param name="maxDigitalValue">对应数值 最大值</param>
    public SignalToDigitalProcessor(decimal minSignalValue, decimal maxSignalValue, decimal minDigitalValue, decimal maxDigitalValue)
    {
        if (minSignalValue >= maxSignalValue)
            throw new ArgumentException("minSignalValue 必须小于 maxSignalValue", nameof(minSignalValue));
        if (minDigitalValue >= maxDigitalValue)
            throw new ArgumentException("minDigitalValue 必须小于 maxDigitalValue", nameof(minDigitalValue));

        _minSignalValue = minSignalValue;
        _maxSignalValue = maxSignalValue;
        _minDigitalValue = minDigitalValue;
        _maxDigitalValue = maxDigitalValue;
        _inputScaleFactor = (maxDigitalValue - minDigitalValue) / (maxSignalValue - minSignalValue);
        _outputScaleFactor = (maxSignalValue - minSignalValue) / (maxDigitalValue - minDigitalValue);
    }

    public decimal InputDataProcess(decimal inputValue)
    {
        if (inputValue <= _minSignalValue)
            return _minDigitalValue;
        if (inputValue >= _maxSignalValue)
            return _maxDigitalValue;
        return _inputScaleFactor * (inputValue - _minSignalValue) + _minDigitalValue;
    }

    public decimal OutputDataProcess(decimal outputValue)
    {
        if (outputValue <= _minDigitalValue)
            return _minSignalValue;
        if (outputValue >= _maxDigitalValue)
            return _maxSignalValue;
        return _outputScaleFactor * (outputValue - _minDigitalValue) + _minSignalValue;
    }
}
