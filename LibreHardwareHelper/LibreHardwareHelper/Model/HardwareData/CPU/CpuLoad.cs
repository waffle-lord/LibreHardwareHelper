using LibreHardwareMonitor.Hardware;
using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.CPU
{
    public class CpuLoad : INotifyPropertyChanged
    {
        private LibreHardwareHelper _helper;

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

        public CpuLoad(IHardware cpu, LibreHardwareHelper helper)
        {
            if (cpu == null) return;

            if (cpu.HardwareType != HardwareType.Cpu) return;

            _helper = helper;

            foreach (ISensor s in cpu.Sensors)
            {
                if (s.SensorType != SensorType.Load) continue;

                if (s.Name == "CPU Total")
                {
                    Total = s.Value ?? 0;
                    continue;
                }
            }
        }

        /// <summary>
        /// Update this <see cref="CpuLoad"/> objects data
        /// </summary>
        /// <param name="DontQueryHardware">Update the values of this object if they differ, but don't ask the hardware to update</param>
        public void Update(bool DontQueryHardware = false)
        {
            CpuLoad tempLoads = _helper.GetCpuLoad(null, DontQueryHardware);

            Total = tempLoads.Total;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
