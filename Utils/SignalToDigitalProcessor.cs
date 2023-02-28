namespace Utils;

public class SignalToDigitalProcessor
{
    /// <summary>电流/电压 最小值</summary>
    private readonly decimal _minSignalValue;

    /// <summary>电流/电压 最大值</summary>
    private readonly decimal _maxSignalValue;

    /// <summary>对应数值 最小值</summary>
    private readonly decimal _minDigitalValue;

    /// <summary>对应数值 最大值</summary>
    private readonly decimal _maxDigitalValue;

    public SignalToDigitalProcessor(decimal minSignalValue, decimal maxSignalValue, decimal minDigitalValue, decimal maxDigitalValue)
    {
        this._minSignalValue = minSignalValue;
        this._maxSignalValue = maxSignalValue;
        this._minDigitalValue = minDigitalValue;
        this._maxDigitalValue = maxDigitalValue;
    }

    public decimal InputDataProcess(decimal inputValue)
    {
        return (_maxDigitalValue - _minDigitalValue)
               / (_maxSignalValue - _minSignalValue)
               * (inputValue - _minSignalValue)
               + _minDigitalValue;
    }

    public decimal OutputDataProcess(decimal outputValue)
    {
        if (outputValue <= _minDigitalValue)
        {
            return _minSignalValue;
        }
        else if (outputValue >= _maxDigitalValue)
        {
            return _maxSignalValue;
        }
        else
        {
            return (_maxSignalValue - _minSignalValue)
                   / (_maxDigitalValue - _minDigitalValue)
                   * (outputValue - _minDigitalValue)
                   + _minSignalValue;
        }
    }
}
