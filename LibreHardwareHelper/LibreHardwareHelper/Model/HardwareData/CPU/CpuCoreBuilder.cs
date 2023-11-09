namespace LibreHardware_Helper.Model.HardwareData.CPU;

//TODO - use builder pattern
internal class CpuCoreBuilder
{
    public int Number { get; set; }
    public string Name { get; set; }
    public float Temp { get; set; }
    public float Load { get; set; }
    public float ClockSpeed { get; set; }
    public float TjMaxDistance { get; set; }

    public CpuCoreBuilder WithNumber(int number)
    {
        Number = number;
        return this;
    }

    public CpuCoreBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public CpuCoreBuilder WithTemp(float temp)
    {
        Temp = temp;
        return this;
    }

    public CpuCoreBuilder WithLoad(float load)
    {
        Load = load;
        return this;
    }

    public CpuCoreBuilder WithClockSpeed(float clockSpeed)
    {
        ClockSpeed = clockSpeed;
        return this;
    }

    public CpuCoreBuilder WithTjMax(float tjmax)
    {
        TjMaxDistance = tjmax;
        return this;
    }

    public CpuCore Build()
    {
        return new CpuCore(Number, Name, Temp, Load, ClockSpeed, TjMaxDistance);
    }
}