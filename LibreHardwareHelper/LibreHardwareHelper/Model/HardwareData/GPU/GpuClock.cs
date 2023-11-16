using System;
using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.GPU;

public class GpuClock : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;
    private float _core;

    private float _memory;

    private float _shader;

    private float _video;

    public GpuClock(IHardware gpu, LibreHardwareHelper helper)
    {
        if (gpu == null) return;

        if (gpu.HardwareType != HardwareType.GpuNvidia && gpu.HardwareType != HardwareType.GpuAmd && gpu.HardwareType != HardwareType.GpuIntel)
            throw new ArgumentException("provided arg is not supported gpu hardware");

        _helper = helper;

        foreach (var s in gpu.Sensors)
        {
            if (s.SensorType != SensorType.Clock) continue;

            switch (s.Name)
            {
                case "GPU Core":
                {
                    Core = s.Value ?? 0;
                    break;
                }
                case "GPU Memory":
                {
                    Memory = s.Value ?? 0;
                    break;
                }
                case "GPU Shader":
                {
                    Shader = s.Value ?? 0;
                    break;
                }
                case "GPU Video":
                {
                    Video = s.Value ?? 0;
                    break;
                }
            }
        }
    }

    public float Core
    {
        get => _core;
        private set => RaiseAndSetIfChanged(ref _core, value);
    }

    public float Memory
    {
        get => _memory;
        private set => RaiseAndSetIfChanged(ref _memory, value);
    }

    public float Shader
    {
        get => _shader;
        private set => RaiseAndSetIfChanged(ref _shader, value);
    }

    public float Video
    {
        get => _video;
        private set => RaiseAndSetIfChanged(ref _video, value);
    }

    public void Update()
    {
        var clock = _helper.GetGpuClock();

        Core = clock.Core;
        Memory = clock.Memory;
        Shader = clock.Shader;
        Video = clock.Video;
    }
}