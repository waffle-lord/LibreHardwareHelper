using System;
using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.GPU;
/*
    -/gpu-nvidia/0/load/0        :: GPU Core - 4
    -/gpu-nvidia/0/load/1        :: GPU Memory Controller - 1
    -/gpu-nvidia/0/load/2        :: GPU Video Engine - 0
    -/gpu-nvidia/0/load/3        :: GPU Bus - 0
    -/gpu-nvidia/0/load/4        :: GPU Memory - 18.602753
    -/gpu-nvidia/0/load/5        :: GPU Power - 22.81
    -/gpu-nvidia/0/load/6        :: GPU Board Power - 22.49
    /gpu-nvidia/0/load/7        :: D3D 3D - 0
    /gpu-nvidia/0/load/11       :: D3D Overlay - 0
    /gpu-nvidia/0/load/13       :: D3D Video Decode - 0
    /gpu-nvidia/0/load/8        :: D3D Copy - 0
    /gpu-nvidia/0/load/9        :: D3D Copy - 0
    /gpu-nvidia/0/load/10       :: D3D Copy - 0
    /gpu-nvidia/0/load/12       :: D3D Security - 0
    /gpu-nvidia/0/load/14       :: D3D Video Encode - 0
    /gpu-nvidia/0/load/15       :: D3D VR - 0
*/

public class GpuLoad : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;
    private float _boardPower;


    private float _bus;

    private float _core;


    private float _memory;


    private float _memoryController;


    private float _power;


    private float _videoEngine;

    public GpuLoad(IHardware gpu, LibreHardwareHelper helper)
    {
        if (gpu.HardwareType != HardwareType.GpuNvidia && gpu.HardwareType != HardwareType.GpuAmd)
            throw new ArgumentException("provided arg is not supported gpu hardware");

        _helper = helper;

        foreach (var s in gpu.Sensors)
        {
            if (s.SensorType != SensorType.Load) continue;

            switch (s.Name)
            {
                case "GPU Core":
                {
                    Core = s.Value ?? 0;
                    continue;
                }
                case "GPU Memory Controller":
                {
                    MemoryController = s.Value ?? 0;
                    continue;
                }
                case "GPU Video Engine":
                {
                    VideoEngine = s.Value ?? 0;
                    break;
                }
                case "GPU Memory":
                {
                    Memory = s.Value ?? 0;
                    break;
                }
                case "GPU Bus":
                {
                    Bus = s.Value ?? 0;
                    break;
                }
                case "GPU Power":
                {
                    Power = s.Value ?? 0;
                    break;
                }
                case "GPU Board Power":
                {
                    BoardPower = s.Value ?? 0;
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

    public float MemoryController
    {
        get => _memoryController;
        private set => RaiseAndSetIfChanged(ref _memoryController, value);
    }

    public float VideoEngine
    {
        get => _videoEngine;
        private set => RaiseAndSetIfChanged(ref _videoEngine, value);
    }

    public float Bus
    {
        get => _bus;
        private set => RaiseAndSetIfChanged(ref _bus, value);
    }

    public float Memory
    {
        get => _memory;
        private set => RaiseAndSetIfChanged(ref _memory, value);
    }

    public float Power
    {
        get => _power;
        private set => RaiseAndSetIfChanged(ref _power, value);
    }

    public float BoardPower
    {
        get => _boardPower;
        private set => RaiseAndSetIfChanged(ref _boardPower, value);
    }

    public void Update()
    {
        var loads = _helper.GetGpuLoad();

        Core = loads.Core;
        MemoryController = loads.MemoryController;
        VideoEngine = loads.VideoEngine;
        Memory = loads.Memory;
        Bus = loads.Bus;
        Power = loads.Power;
        BoardPower = loads.BoardPower;
    }
}