using System;
using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.GPU;

public class GpuClock : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;
    private float _Core;

    private float _Memory;

    private float _Shader;

    private float _Video;

    public GpuClock(IHardware gpu, LibreHardwareHelper helper)
    {
        if (gpu == null) return;

        if (gpu.HardwareType != HardwareType.GpuNvidia && gpu.HardwareType != HardwareType.GpuAmd)
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
        get => _Core;
        private set => RaiseAndSetIfChanged(ref _Core, value);
    }

    public float Memory
    {
        get => _Memory;
        private set => RaiseAndSetIfChanged(ref _Memory, value);
    }

    public float Shader
    {
        get => _Shader;
        private set => RaiseAndSetIfChanged(ref _Shader, value);
    }

    public float Video
    {
        get => _Video;
        private set => RaiseAndSetIfChanged(ref _Video, value);
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