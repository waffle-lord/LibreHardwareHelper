namespace LibreHardware_Helper.Model.HardwareData.CPU;

public class CoreInfo : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;

    private string _Name;

    private int _Number;

    private float _Value;

    public CoreInfo(string Name, float? Value, LibreHardwareHelper helper)
    {
        _helper = helper;
        Number = _helper.GetCoreNumber(Name);
        this.Name = Name;
        this.Value = Value ?? 0;
    }

    public int Number
    {
        get => _Number;
        private set => RaiseAndSetIfChanged(ref _Number, value);
    }

    public string Name
    {
        get => _Name;
        private set => RaiseAndSetIfChanged(ref _Name, value);
    }

    public float Value
    {
        get => _Value;
        private set => RaiseAndSetIfChanged(ref _Value, value);
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