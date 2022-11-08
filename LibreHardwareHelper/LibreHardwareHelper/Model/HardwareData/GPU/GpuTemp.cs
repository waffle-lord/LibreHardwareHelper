using LibreHardwareMonitor.Hardware;
using System;
using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.GPU
{
    public class GpuTemp : INotifyPropertyChanged
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

        public GpuTemp(IHardware gpu, LibreHardwareHelper helper)
        {
            if (gpu == null) return;

            if (gpu.HardwareType != HardwareType.GpuNvidia && gpu.HardwareType != HardwareType.GpuAmd)
                throw new ArgumentException($"provided arg is not supported gpu hardware");

            _helper = helper;

            foreach (ISensor s in gpu.Sensors)
            {
                if (s.SensorType != SensorType.Temperature) continue;

                if (s.Name == "GPU Core")
                {
                    Core = s.Value ?? 0;
                    return;
                }
            }
        }

        public void Update()
        {
            var temp = _helper.GetGpuTemp();

            Core = temp.Core;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
