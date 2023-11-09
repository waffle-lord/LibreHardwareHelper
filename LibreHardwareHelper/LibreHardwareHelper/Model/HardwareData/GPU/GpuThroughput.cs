using System;
using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.GPU;

public class GpuThroughput : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;

    private float _pcIeReceive;

    private float _pcIeTransmit;


    public GpuThroughput(IHardware gpu, LibreHardwareHelper helper)
    {
        if (gpu == null) return;

        if (gpu.HardwareType != HardwareType.GpuNvidia && gpu.HardwareType != HardwareType.GpuAmd)
            throw new ArgumentException("provided arg is not supported gpu hardware");

        _helper = helper;

        foreach (var s in gpu.Sensors)
        {
            if (s.SensorType != SensorType.Throughput) continue;

            switch (s.Name)
            {
                case "GPU PCIe Rx":
                {
                    PcIeReceive = s.Value ?? 0;
                    continue;
                }
                case "GPU PCIe Tx":
                {
                    PcIeTrasmit = s.Value ?? 0;
                    continue;
                }
            }
        }
    }

    public float PcIeTrasmit
    {
        get => _pcIeTransmit;
        private set => RaiseAndSetIfChanged(ref _pcIeTransmit, value);
    }

    public float PcIeReceive
    {
        get => _pcIeReceive;
        private set => RaiseAndSetIfChanged(ref _pcIeReceive, value);
    }

    public void Update()
    {
        var throughput = _helper.GetGpuThroughput();

        PcIeReceive = throughput.PcIeReceive;
        PcIeTrasmit = throughput.PcIeTrasmit;
    }
}