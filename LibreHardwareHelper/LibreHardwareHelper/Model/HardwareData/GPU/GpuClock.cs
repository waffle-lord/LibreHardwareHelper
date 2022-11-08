using LibreHardwareMonitor.Hardware;
using System;
using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.GPU
{
    public class GpuClock : INotifyPropertyChanged
    {
        private LibreHardwareHelper _helper;

        private float _Core;
        public float Core
        {
            get => _Core;
            private set
            {
                if (_Core != value)
                {
                    _Core = value;
                    RaisePropertyChanged(nameof(Core));
                }
            }
        }

        private float _Memory;
        public float Memory
        {
            get => _Memory;
            private set
            {
                if (_Memory != value)
                {
                    _Memory = value;
                    RaisePropertyChanged(nameof(Memory));
                }
            }
        }

        private float _Shader;
        public float Shader
        {
            get => _Shader;
            private set
            {
                if (_Shader != value)
                {
                    _Shader = value;
                    RaisePropertyChanged(nameof(_Shader));
                }
            }
        }

        private float _Video;
        public float Video
        {
            get => _Video;
            private set
            {
                if (_Video != value)
                {
                    _Video = value;
                    RaisePropertyChanged(nameof(Video));
                }
            }
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
