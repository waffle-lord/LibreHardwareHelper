using LibreHardwareMonitor.Hardware;
using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.CPU
{
    public class CpuTemp : INotifyPropertyChanged
    {
        private LibreHardwareHelper _helper;

        private float _CoreAverge;
        public float CoreAverage
        {
            get => _CoreAverge;
            private set
            {
                if (_CoreAverge != value)
                {
                    _CoreAverge = value;
                    RaisePropertyChange(nameof(CoreAverage));
                }
            }
        }

        private float _PackageTemp;
        public float PackageTemp
        {
            get => _PackageTemp;
            private set
            {
                if (_PackageTemp != value)
                {
                    _PackageTemp = value;
                    RaisePropertyChange(nameof(PackageTemp));
                }
            }
        }

        private float _CoreMaxTemp;
        public float CoreMaxTemp
        {
            get => _CoreMaxTemp;
            private set
            {
                if (_CoreMaxTemp != value)
                {
                    _CoreMaxTemp = value;
                    RaisePropertyChange(nameof(CoreMaxTemp));
                }
            }
        }

        public CpuTemp(IHardware cpu, LibreHardwareHelper helper)
        {
            if (cpu == null) return;

            if (cpu.HardwareType != HardwareType.Cpu) return;

            _helper = helper;

            foreach (ISensor s in cpu.Sensors)
            {
                if (s.SensorType != SensorType.Temperature) continue;

                switch (s.Name)
                {
                    case "CPU Package":
                        {
                            //add pacakge temp
                            PackageTemp = s.Value ?? 0;
                            break;
                        }
                    case "Core Max":
                        {
                            //add max temp
                            CoreMaxTemp = s.Value ?? 0;
                            break;
                        }
                    case "Core Average":
                        {
                            //add cpu average temp
                            CoreAverage = s.Value ?? 0;
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Update this <see cref="CpuTemp"/> objects data.
        /// </summary>
        /// <param name="DontQueryHardware">Update the values of this object if they differ, but don't ask the hardware to update</param>
        public void Update(bool DontQueryHardware = false)
        {
            CpuTemp tempTemps = _helper.GetCpuTemp(null, DontQueryHardware);

            CoreAverage = tempTemps.CoreAverage;
            PackageTemp = tempTemps.PackageTemp;
            CoreMaxTemp = tempTemps.CoreMaxTemp;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChange(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
