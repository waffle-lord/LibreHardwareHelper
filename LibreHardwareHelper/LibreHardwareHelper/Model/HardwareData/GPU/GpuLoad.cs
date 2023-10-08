using LibreHardwareMonitor.Hardware;
using System;

namespace LibreHardware_Helper.Model.HardwareData.GPU
{
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
        private LibreHardwareHelper _helper;

        private float _Core;
        public float Core
        {
            get => _Core;
            private set => RaiseAndSetIfChanged(ref _Core, value);
        }


        private float _MemoryController;
        public float MemoryController
        {
            get => _MemoryController;
            private set => RaiseAndSetIfChanged(ref _MemoryController, value);
        }


        private float _VideoEngine;
        public float VideoEngine
        {
            get => _VideoEngine;
            private set => RaiseAndSetIfChanged(ref _VideoEngine, value);
        }


        private float _Bus;
        public float Bus
        {
            get => _Bus;
            private set => RaiseAndSetIfChanged(ref _Bus, value);
        }


        private float _Memory;
        public float Memory
        {
            get => _Memory;
            private set => RaiseAndSetIfChanged(ref _Memory, value);
        }


        private float _Power;
        public float Power
        {
            get => _Power;
            private set => RaiseAndSetIfChanged(ref _Power, value);
        }


        private float _BoardPower;
        public float BoardPower
        {
            get => _BoardPower;
            private set => RaiseAndSetIfChanged(ref _BoardPower, value);
        }

        public GpuLoad(IHardware gpu, LibreHardwareHelper helper)
        {
            if (gpu.HardwareType != HardwareType.GpuNvidia && gpu.HardwareType != HardwareType.GpuAmd)
                throw new ArgumentException($"provided arg is not supported gpu hardware");

            _helper = helper;

            foreach (ISensor s in gpu.Sensors)
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
}
