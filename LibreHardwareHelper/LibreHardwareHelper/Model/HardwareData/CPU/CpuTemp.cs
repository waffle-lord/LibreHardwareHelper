using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.CPU;

public class CpuTemp : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;
    private float _CoreAverge;

    private float _CoreMaxTemp;

    private float _PackageTemp;

    public CpuTemp(IHardware cpu, LibreHardwareHelper helper)
    {
        if (cpu == null) return;

        if (cpu.HardwareType != HardwareType.Cpu) return;

        _helper = helper;

        foreach (var s in cpu.Sensors)
        {
            if (s.SensorType != SensorType.Temperature) continue;

            switch (s.Name)
            {
                case "CPU Package":
                {
                    //add pacakge temp
                    PackageTemp = s.Value ?? 0;
                    break;
                }
                case "Core Max":
                {
                    //add max temp
                    CoreMaxTemp = s.Value ?? 0;
                    break;
                }
                case "Core Average":
                {
                    //add cpu average temp
                    CoreAverage = s.Value ?? 0;
                    break;
                }
            }
        }
    }

    public float CoreAverage
    {
        get => _CoreAverge;
        private set => RaiseAndSetIfChanged(ref _CoreAverge, value);
    }

    public float PackageTemp
    {
        get => _PackageTemp;
        private set => RaiseAndSetIfChanged(ref _PackageTemp, value);
    }

    public float CoreMaxTemp
    {
        get => _CoreMaxTemp;
        private set => RaiseAndSetIfChanged(ref _CoreMaxTemp, value);
    }

    /// <summary>
    ///     Update this <see cref="CpuTemp" /> objects data.
    /// </summary>
    /// <param name="dontQueryHardware">Update the values of this object if they differ, but don't ask the hardware to update</param>
    public void Update(bool dontQueryHardware = false)
    {
        var tempTemps = _helper.GetCpuTemp(null, dontQueryHardware);

        CoreAverage = tempTemps.CoreAverage;
        PackageTemp = tempTemps.PackageTemp;
        CoreMaxTemp = tempTemps.CoreMaxTemp;
    }
}