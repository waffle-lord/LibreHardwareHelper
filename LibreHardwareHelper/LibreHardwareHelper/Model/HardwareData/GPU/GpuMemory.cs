using LibreHardwareMonitor.Hardware;
using System;
using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.GPU
{
    public class GpuMemory : INotifyPropertyChanged
    {
        private LibreHardwareHelper _helper;

        private float _D3DDedicatedUsed;
        public float D3DDedicatedUsed
        {
            get => _D3DDedicatedUsed;
            private set
            {
                if (_D3DDedicatedUsed != value)
                {
                    _D3DDedicatedUsed = value;

                    RaisePropertyChanged(nameof(D3DDedicatedUsed));
                }
            }
        }

        private float _D3DSharedUsed;
        public float D3DSharedUsed
        {
            get => _D3DSharedUsed;
            private set
            {
                if (_D3DSharedUsed != value)
                {
                    _D3DSharedUsed = value;

                    RaisePropertyChanged(nameof(D3DSharedUsed));
                }
            }
        }


        private float _AmountUsed;
        public float AmountUsed
        {
            get => _AmountUsed;
            private set
            {
                if (_AmountUsed != value)
                {
                    _AmountUsed = value;

                    RaisePropertyChanged(nameof(AmountUsed));
                }
            }
        }


        private float _AmountAvailable;
        public float AmountAvailable
        {
            get => _AmountAvailable;
            private set
            {
                if (_AmountAvailable != value)
                {
                    _AmountAvailable = value;

                    RaisePropertyChanged(nameof(AmountAvailable));
                }
            }
        }

        private float _Total;
        public float Total
        {
            get => _Total;
            private set
            {
                if (_Total != value)
                {
                    _Total = value;

                    RaisePropertyChanged(nameof(Total));
                }
            }
        }

        public float PercentUsed => (float)Math.Floor(_AmountUsed / _Total * 100);
        public float PercentAvailable => (float)Math.Floor(_AmountAvailable / _Total * 100);

        public GpuMemory(IHardware gpu, LibreHardwareHelper helper)
        {
            if (gpu.HardwareType != HardwareType.GpuNvidia && gpu.HardwareType != HardwareType.GpuAmd)
                throw new ArgumentException($"provided arg is not supported gpu hardware");

            _helper = helper;

            foreach (ISensor s in gpu.Sensors)
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

        public void Update()
        {
            var memory = _helper.GetGpuMemory();

            D3DDedicatedUsed = memory.D3DDedicatedUsed;
            D3DSharedUsed = memory.D3DSharedUsed;
            Total = memory.Total;
            AmountAvailable = memory.AmountAvailable;
            AmountUsed = memory.AmountUsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
