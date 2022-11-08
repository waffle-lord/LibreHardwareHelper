using LibreHardwareMonitor.Hardware;
using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.CPU
{
    public class CpuPower : INotifyPropertyChanged
    {
        private LibreHardwareHelper _helper;

        private float _Package;
        public float Package
        {
            get => _Package;
            private set
            {
                if (_Package != value)
                {
                    _Package = value;
                    RaisePropertyChanged(nameof(Package));
                }
            }
        }

        private float _Cores;
        public float Cores
        {
            get => _Cores;
            private set
            {
                if (_Cores != value)
                {
                    _Cores = value;
                    RaisePropertyChanged(nameof(Cores));
                }
            }
        }

        private float _Graphics;
        public float Graphics
        {
            get => _Graphics;
            private set
            {
                if (_Graphics != value)
                {
                    _Graphics = value;
                    RaisePropertyChanged(nameof(Graphics));
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

        public CpuPower(IHardware cpu, LibreHardwareHelper helper)
        {
            if (cpu == null) return;

            if (cpu.HardwareType != HardwareType.Cpu) return;

            _helper = helper;

            foreach (ISensor s in cpu.Sensors)
            {
                switch (s.Name)
                {
                    case "CPU Package":
                        {
                            Package = s.Value ?? 0;
                            break;
                        }
                    case "CPU Cores":
                        {
                            Cores = s.Value ?? 0;
                            break;
                        }
                    case "CPU Graphics":
                        {
                            Graphics = s.Value ?? 0;
                            break;
                        }
                    case "CPU Memory":
                        {
                            Memory = s.Value ?? 0;
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Update this <see cref="CpuPower"/> objects data.
        /// </summary>
        /// <param name="DontQueryHardware">Update the values of this object if they differ, but don't ask the hardware to update</param>
        public void Update(bool DontQueryHardware = false)
        {
            CpuPower tempPower = _helper.GetCpuPower(null, DontQueryHardware);

            Package = tempPower.Package;
            Cores = tempPower.Cores;
            Graphics = tempPower.Graphics;
            Memory = tempPower.Memory;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
