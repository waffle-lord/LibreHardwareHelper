using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.CPU;

public class CpuTemp : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;
    private float _coreAverage;

    private float _coreMaxTemp;

    private float _packageTemp;

    // TODO: opinion on toLowering the strings?
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
                case "Core (Tctl/Tdie)":
                {
                    CoreAverage = s.Value ?? 0;
                    break;
                }
                case "CCD1 (Tdie)":
                {
                    PackageTemp = s.Value ?? 0;
                    break;
                }
            }
        }
    }

    public float CoreAverage
    {
        get => _coreAverage;
        private set => RaiseAndSetIfChanged(ref _coreAverage, value);
    }

    public float PackageTemp
    {
        get => _packageTemp;
        private set => RaiseAndSetIfChanged(ref _packageTemp, value);
    }

    public float CoreMaxTemp
    {
        get => _coreMaxTemp;
        private set => RaiseAndSetIfChanged(ref _coreMaxTemp, value);
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