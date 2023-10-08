using LibreHardwareMonitor.Hardware;
using System;

namespace LibreHardware_Helper.Model.HardwareData.GPU
{
    public class GpuClock : PropertyNotifierBase
    {
        private LibreHardwareHelper _helper;

        private float _Core;
        public float Core
        {
            get => _Core;
            private set => RaiseAndSetIfChanged(ref _Core, value);
        }

        private float _Memory;
        public float Memory
        {
            get => _Memory;
            private set => RaiseAndSetIfChanged(ref _Memory, value);
        }

        private float _Shader;
        public float Shader
        {
            get => _Shader;
            private set => RaiseAndSetIfChanged(ref _Shader, value);
        }

        private float _Video;
        public float Video
        {
            get => _Video;
            private set => RaiseAndSetIfChanged(ref _Video, value);
        }

        public GpuClock(IHardware gpu, LibreHardwareHelper helper)
        {
            if (gpu == null) return;

            if (gpu.HardwareType != HardwareType.GpuNvidia && gpu.HardwareType != HardwareType.GpuAmd)
                throw new ArgumentException($"provided arg is not supported gpu hardware");

            _helper = helper;

            foreach (ISensor s in gpu.Sensors)
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

        public void Update()
        {
            var clock = _helper.GetGpuClock();

            Core = clock.Core;
            Memory = clock.Memory;
            Shader = clock.Shader;
            Video = clock.Video;
        }
    }
}
