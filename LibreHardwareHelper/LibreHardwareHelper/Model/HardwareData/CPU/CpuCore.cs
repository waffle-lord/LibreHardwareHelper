namespace LibreHardware_Helper.Model.HardwareData.CPU;

/// <summary>
///     All data relevent to a single CPU core
/// </summary>
public class CpuCore : PropertyNotifierBase
{
    private float _clockSpeed;

    private float _load;

    private string _name;
    private int _number;

    private float _temp;

    private float _tjMaxDistance;

    public CpuCore(int number, string name, float temp, float load, float clockSpeed, float tjMaxDistance)
    {
        this.Number = number;
        this.Name = name;
        this.Temp = temp;
        this.Load = load;
        this.ClockSpeed = clockSpeed;
        this.TjMaxDistance = tjMaxDistance;
    }

    public int Number
    {
        get => _number;
        private set => RaiseAndSetIfChanged(ref _number, value);
    }

    public string Name
    {
        get => _name;
        private set => RaiseAndSetIfChanged(ref _name, value);
    }

    public float Temp
    {
        get => _temp;
        private set
        {
            if (_temp != value)
            {
                _temp = value;
                RaisePropertyChanged(nameof(Temp));
                RaisePropertyChanged(nameof(TempPercentage));
            }
        }
    }

    public float TempPercentage
    {
        get
        {
            var maxTemp = Temp + TjMaxDistance;

            return Temp / maxTemp * 100;
        }
    }

    public float Load
    {
        get => _load;
        private set => RaiseAndSetIfChanged(ref _load, value);
    }

    public float ClockSpeed
    {
        get => _clockSpeed;
        private set => RaiseAndSetIfChanged(ref _clockSpeed, value);
    }

    public float TjMaxDistance
    {
        get => _tjMaxDistance;
        private set => RaiseAndSetIfChanged(ref _tjMaxDistance, value);
    }

    public float MaxTemp => Temp + TjMaxDistance;

    /// <summary>
    ///     Update this <see cref="CpuCore" /> objects data.
    /// </summary>
    /// <param name="DontQueryHardware">Update the values of this object if they differ, but don't ask the hardware to update</param>
    public void Update(CpuCore core)
    {
        if (core.Name != Name) return;

        Temp = core.Temp;
        Load = core.Load;
        ClockSpeed = core.ClockSpeed;
        TjMaxDistance = core.TjMaxDistance;
    }
}