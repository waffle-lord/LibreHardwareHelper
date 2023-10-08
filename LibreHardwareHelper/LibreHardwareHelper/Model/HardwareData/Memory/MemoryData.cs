using LibreHardwareMonitor.Hardware;
using System;
using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.Memory
{
    public class MemoryData : PropertyNotifierBase
    {
        private LibreHardwareHelper _helper;

        private float _PercentUsed;
        public float PercentUsed
        {
            get => _PercentUsed;
            private set
            {
                if (_PercentUsed != value)
                {
                    _PercentUsed = value;
                    RaisePropertyChanged(nameof(PercentUsed));
                    RaisePropertyChanged(nameof(PercentAvailable));
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
                    RaisePropertyChanged(nameof(Total));
                }
            }
        }

        private float _AmountAvailable;
        public float AmountAvailable
        {
            get => _AmountAvailable;
            private set => RaiseAndSetIfChanged(ref _AmountAvailable, value);
        }

        private float _VirtualPercentUsed;
        public float VirtualPercentUsed
        {
            get => _VirtualPercentUsed;
            private set
            {
                if (_VirtualPercentUsed != value)
                {
                    _VirtualPercentUsed = value;
                    RaisePropertyChanged(nameof(VirtualPercentUsed));
                    RaisePropertyChanged(nameof(VirtualPercentAvailable));
                }
            }
        }

        private float _VirtualAmountUsed;
        public float VirtualAmountUsed
        {
            get => _VirtualAmountUsed;
            private set
            {
                if (_VirtualAmountUsed != value)
                {
                    _VirtualAmountUsed = value;
                    RaisePropertyChanged(nameof(VirtualAmountUsed));
                    RaisePropertyChanged(nameof(VirtualTotal));
                }
            }
        }

        private float _VirtualAmountAvailable;
        public float VirtualAmountAvailable
        {
            get => _VirtualAmountAvailable;
            private set => RaiseAndSetIfChanged(ref _VirtualAmountAvailable, value);
        }

        public float Total => AmountUsed + AmountAvailable;
        public float VirtualTotal => VirtualAmountUsed + VirtualAmountAvailable;

        public float PercentAvailable => 100 - PercentUsed;
        public float VirtualPercentAvailable => 100 - VirtualPercentUsed;

        public MemoryData(IHardware Memory, LibreHardwareHelper helper)
        {
            if (Memory.HardwareType != HardwareType.Memory) return;

            _helper = helper;

            foreach (ISensor s in Memory.Sensors)
            {
                switch (s.Name)
                {
                    case "Memory":
                        {
                            PercentUsed = s.Value ?? 0;
                            break;
                        }
                    case "Memory Used":
                        {
                            AmountUsed = s.Value ?? 0;
                            break;
                        }
                    case "Memory Available":
                        {
                            AmountAvailable = s.Value ?? 0;
                            break;
                        }
                    case "Virtual Memory":
                        {
                            VirtualPercentUsed = s.Value ?? 0;
                            break;
                        }
                    case "Virtual Memory Used":
                        {
                            VirtualAmountUsed = s.Value ?? 0;
                            break;
                        }
                    case "Virtual Memory Available":
                        {
                            VirtualAmountAvailable = s.Value ?? 0;
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Update this <see cref="MemoryData"/> objects data
        /// </summary>
        public void Update()
        {
            MemoryData memData = _helper.GetMemoryData();

            PercentUsed = memData.PercentUsed;
            AmountUsed = memData.AmountUsed;
            AmountAvailable = memData.AmountAvailable;

            VirtualPercentUsed = memData.VirtualPercentUsed;
            VirtualAmountUsed = memData.VirtualAmountUsed;
            VirtualAmountAvailable = memData.VirtualAmountAvailable;

            RaiseDataUpdated();
        }

        public event EventHandler DataUpdated;
        protected virtual void RaiseDataUpdated()
        {
            DataUpdated?.Invoke(this, null);
        }
    }
}
