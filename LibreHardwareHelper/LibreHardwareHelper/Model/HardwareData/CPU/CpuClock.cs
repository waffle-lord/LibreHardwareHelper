using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.CPU;

public class CpuClock : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;
    private float _busSpeed;

    public CpuClock(IHardware cpu, LibreHardwareHelper helper)
    {
        if (cpu == null) return;

        if (cpu.HardwareType != HardwareType.Cpu) return;

        _helper = helper;

        foreach (var s in cpu.Sensors)
        {
            if (s.SensorType != SensorType.Clock) continue;

            if (s.Name == "Bus Speed") BusSpeed = s.Value ?? 0;
        }
    }

    public float BusSpeed
    {
        get => _busSpeed;
        private set => RaiseAndSetIfChanged(ref _busSpeed, value);
    }

    /// <summary>
    ///     Update this <see cref="CpuClock" /> objects data.
    /// </summary>
    /// <param name="dontQueryHardware">Update the values of this object if they differ, but don't ask the hardware to update</param>
    public void Update(bool dontQueryHardware = false)
    {
        var tempClocks = _helper.GetCpuClock(null, dontQueryHardware);

        BusSpeed = tempClocks.BusSpeed;
    }
}