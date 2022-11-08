using LibreHardwareMonitor.Hardware;
using System;
using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.Memory
{
    public class MemoryData : INotifyPropertyChanged
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
            private set
            {
                if (_AmountAvailable != value)
                {
                    _AmountAvailable = value;
                    RaisePropertyChanged(nameof(AmountAvailable));
                }
            }
        }

        public float Total => AmountUsed + AmountAvailable;

        public float PercentAvailable => 100 - PercentUsed;

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

            RaiseDataUpdated();
        }

        public event EventHandler DataUpdated;
        protected virtual void RaiseDataUpdated()
        {
            DataUpdated?.Invoke(this, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
