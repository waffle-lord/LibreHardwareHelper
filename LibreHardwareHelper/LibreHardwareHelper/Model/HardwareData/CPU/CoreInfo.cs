namespace LibreHardware_Helper.Model.HardwareData.CPU;

// TODO: this is currently never used
public class CoreInfo : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;

    private string _name;

    private int _number;

    private float _value;

    public CoreInfo(string name, float? value, LibreHardwareHelper helper)
    {
        _helper = helper;
        Number = _helper.GetCoreNumber(name);
        this.Name = name;
        this.Value = value ?? 0;
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

    public float Value
    {
        get => _value;
        private set => RaiseAndSetIfChanged(ref _value, value);
    }

    /// <summary>
    ///     Update this <see cref="CoreInfo" /> objects data.
    /// </summary>
    public void Update(CoreInfo coreInfo)
    {
        Number = coreInfo.Number;
        Name = coreInfo.Name;
        Value = coreInfo.Value;
    }
}