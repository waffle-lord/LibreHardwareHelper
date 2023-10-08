using LibreHardwareMonitor.Hardware;
using System;
using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.GPU
{
    /*
        /gpu-nvidia/0/load/0        :: GPU Core - 4
        /gpu-nvidia/0/load/1        :: GPU Memory Controller - 1
        /gpu-nvidia/0/load/2        :: GPU Video Engine - 0
        /gpu-nvidia/0/load/3        :: GPU Bus - 0
        /gpu-nvidia/0/load/4        :: GPU Memory - 18.602753
        /gpu-nvidia/0/load/5        :: GPU Power - 22.81
        /gpu-nvidia/0/load/6        :: GPU Board Power - 22.49
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

    public class GpuLoad : INotifyPropertyChanged
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


        private float _MemoryController;
        public float MemoryController
        {
            get => _MemoryController;
            private set
            {
                if (_MemoryController != value)
                {
                    _MemoryController = value;

                    RaisePropertyChanged(nameof(MemoryController));
                }
            }
        }


        private float _VideoEngine;
        public float VideoEngine
        {
            get => _VideoEngine;
            private set
            {
                if (_VideoEngine != value)
                {
                    _VideoEngine = value;

                    RaisePropertyChanged(nameof(VideoEngine));
                }
            }
        }


        private float _Bus;
        public float Bus
        {
            get => _Bus;
            private set
            {
                if (_Bus != value)
                {
                    _Bus = value;

                    RaisePropertyChanged(nameof(Bus));
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


        private float _Power;
        public float Power
        {
            get => _Power;
            private set
            {
                if (_Power != value)
                {
                    _Power = value;

                    RaisePropertyChanged(nameof(Power));
                }
            }
        }


        private float _BoardPower;
        public float BoardPower
        {
            get => _BoardPower;
            private set
            {
                if (_BoardPower != value)
                {
                    _BoardPower = value;

                    RaisePropertyChanged(nameof(BoardPower));
                }
            }
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
