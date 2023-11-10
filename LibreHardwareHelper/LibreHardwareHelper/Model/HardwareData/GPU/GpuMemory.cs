using System;
using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.GPU;

public class GpuMemory : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;
    private float _amountAvailable;


    private float _amountUsed;

    private float _d3DDedicatedUsed;

    private float _d3DSharedUsed;

    private float _total;

    public GpuMemory(IHardware gpu, LibreHardwareHelper helper)
    {
        if (gpu.HardwareType != HardwareType.GpuNvidia && gpu.HardwareType != HardwareType.GpuAmd && gpu.HardwareType != HardwareType.GpuIntel)
            throw new ArgumentException("provided arg is not supported gpu hardware");

        _helper = helper;

        foreach (var s in gpu.Sensors)
        {
            if (s.SensorType != SensorType.SmallData) continue;

            switch (s.Name)
            {
                case "D3D Dedicated Memory Used":
                {
                    D3DDedicatedUsed = s.Value ?? 0;
                    break;
                }
                case "D3D Shared Memory Used":
                {
                    D3DSharedUsed = s.Value ?? 0;
                    break;
                }
                case "GPU Memory Total":
                {
                    Total = s.Value ?? 0;
                    break;
                }
                case "GPU Memory Free":
                {
                    AmountAvailable = s.Value ?? 0;
                    break;
                }
                case "GPU Memory Used":
                {
                    AmountUsed = s.Value ?? 0;
                    break;
                }
            }
        }
    }

    public float D3DDedicatedUsed
    {
        get => _d3DDedicatedUsed;
        private set => RaiseAndSetIfChanged(ref _d3DDedicatedUsed, value);
    }

    public float D3DSharedUsed
    {
        get => _d3DSharedUsed;
        private set => RaiseAndSetIfChanged(ref _d3DSharedUsed, value);
    }

    public float AmountUsed
    {
        get => _amountUsed;
        private set => RaiseAndSetIfChanged(ref _amountUsed, value);
    }

    public float AmountAvailable
    {
        get => _amountAvailable;
        private set => RaiseAndSetIfChanged(ref _amountAvailable, value);
    }

    public float Total
    {
        get => _total;
        private set => RaiseAndSetIfChanged(ref _total, value);
    }

    public float PercentUsed => (float)Math.Floor(_amountUsed / _total * 100);
    public float PercentAvailable => (float)Math.Floor(_amountAvailable / _total * 100);

    public void Update()
    {
        var memory = _helper.GetGpuMemory();

        D3DDedicatedUsed = memory.D3DDedicatedUsed;
        D3DSharedUsed = memory.D3DSharedUsed;
        Total = memory.Total;
        AmountAvailable = memory.AmountAvailable;
        AmountUsed = memory.AmountUsed;
    }
}