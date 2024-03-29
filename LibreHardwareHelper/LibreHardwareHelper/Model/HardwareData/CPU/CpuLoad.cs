﻿using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.CPU;

public class CpuLoad : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;

    private float _total;

    public CpuLoad(IHardware cpu, LibreHardwareHelper helper)
    {
        if (cpu == null) return;

        if (cpu.HardwareType != HardwareType.Cpu) return;

        _helper = helper;

        foreach (var s in cpu.Sensors)
        {
            if (s.SensorType != SensorType.Load) continue;

            if (s.Name == "CPU Total") Total = s.Value ?? 0;
        }
    }

    public float Total
    {
        get => _total;
        private set => RaiseAndSetIfChanged(ref _total, value);
    }

    /// <summary>
    ///     Update this <see cref="CpuLoad" /> objects data
    /// </summary>
    /// <param name="dontQueryHardware">Update the values of this object if they differ, but don't ask the hardware to update</param>
    public void Update(bool dontQueryHardware = false)
    {
        var tempLoads = _helper.GetCpuLoad(null, dontQueryHardware);

        Total = tempLoads.Total;
    }
}