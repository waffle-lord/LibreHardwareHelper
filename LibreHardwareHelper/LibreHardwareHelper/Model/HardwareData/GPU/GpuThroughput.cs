using System;
using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.GPU;

public class GpuThroughput : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;

    private float _PCIeReceive;

    private float _PCIeTransmit;


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
                    PCIeReceive = s.Value ?? 0;
                    continue;
                }
                case "GPU PCIe Tx":
                {
                    PCIeTrasmit = s.Value ?? 0;
                    continue;
                }
            }
        }
    }

    public float PCIeTrasmit
    {
        get => _PCIeTransmit;
        private set => RaiseAndSetIfChanged(ref _PCIeTransmit, value);
    }

    public float PCIeReceive
    {
        get => _PCIeReceive;
        private set => RaiseAndSetIfChanged(ref _PCIeReceive, value);
    }

    public void Update()
    {
        var throughput = _helper.GetGpuThroughput();

        PCIeReceive = throughput.PCIeReceive;
        PCIeTrasmit = throughput.PCIeTrasmit;
    }
}