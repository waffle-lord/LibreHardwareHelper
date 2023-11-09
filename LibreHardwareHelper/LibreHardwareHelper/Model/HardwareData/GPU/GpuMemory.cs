using System;
using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.GPU;

public class GpuMemory : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;
    private float _AmountAvailable;


    private float _AmountUsed;

    private float _D3DDedicatedUsed;

    private float _D3DSharedUsed;

    private float _Total;

    public GpuMemory(IHardware gpu, LibreHardwareHelper helper)
    {
        if (gpu.HardwareType != HardwareType.GpuNvidia && gpu.HardwareType != HardwareType.GpuAmd)
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
        get => _D3DDedicatedUsed;
        private set => RaiseAndSetIfChanged(ref _D3DDedicatedUsed, value);
    }

    public float D3DSharedUsed
    {
        get => _D3DSharedUsed;
        private set => RaiseAndSetIfChanged(ref _D3DSharedUsed, value);
    }

    public float AmountUsed
    {
        get => _AmountUsed;
        private set => RaiseAndSetIfChanged(ref _AmountUsed, value);
    }

    public float AmountAvailable
    {
        get => _AmountAvailable;
        private set => RaiseAndSetIfChanged(ref _AmountAvailable, value);
    }

    public float Total
    {
        get => _Total;
        private set => RaiseAndSetIfChanged(ref _Total, value);
    }

    public float PercentUsed => (float)Math.Floor(_AmountUsed / _Total * 100);
    public float PercentAvailable => (float)Math.Floor(_AmountAvailable / _Total * 100);

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