using System;
using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.GPU;

public class GpuTemp : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;
    private float _Core;

    public GpuTemp(IHardware gpu, LibreHardwareHelper helper)
    {
        if (gpu == null) return;

        if (gpu.HardwareType != HardwareType.GpuNvidia && gpu.HardwareType != HardwareType.GpuAmd)
            throw new ArgumentException("provided arg is not supported gpu hardware");

        _helper = helper;

        foreach (var s in gpu.Sensors)
        {
            if (s.SensorType != SensorType.Temperature) continue;

            if (s.Name == "GPU Core")
            {
                Core = s.Value ?? 0;
                return;
            }
        }
    }

    public float Core
    {
        get => _Core;
        private set => RaiseAndSetIfChanged(ref _Core, value);
    }

    public void Update()
    {
        var temp = _helper.GetGpuTemp();

        Core = temp.Core;
    }
}