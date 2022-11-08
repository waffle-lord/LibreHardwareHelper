using LibreHardwareMonitor.Hardware;
using System;
using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.GPU
{
    public class GpuThroughput : INotifyPropertyChanged
    {
        private LibreHardwareHelper _helper;

        private float _PCIeTransmit;
        public float PCIeTrasmit
        {
            get => _PCIeTransmit;
            private set
            {
                if (_PCIeTransmit != value)
                {
                    _PCIeTransmit = value;
                    RaisePropertyChanged(nameof(PCIeTrasmit));
                }
            }
        }

        private float _PCIeReceive;
        public float PCIeReceive
        {
            get => _PCIeReceive;
            private set
            {
                if (_PCIeReceive != value)
                {
                    _PCIeReceive = value;
                    RaisePropertyChanged(nameof(PCIeReceive));
                }
            }
        }


        public GpuThroughput(IHardware gpu, LibreHardwareHelper helper)
        {
            if (gpu == null) return;

            if (gpu.HardwareType != HardwareType.GpuNvidia && gpu.HardwareType != HardwareType.GpuAmd)
                throw new ArgumentException($"provided arg is not supported gpu hardware");

            _helper = helper;

            foreach (ISensor s in gpu.Sensors)
            {
                if (s.SensorType != SensorType.Throughput) continue;

                switch (s.Name)
                {
                    case "GPU PCIe Rx":
                        {
                            PCIeReceive = s.Value ?? 0;
                            continue;
                        }
                    case "GPU PCIe Tx":
                        {
                            PCIeTrasmit = s.Value ?? 0;
                            continue;
                        }
                }
            }
        }

        public void Update()
        {
            var throughput = _helper.GetGpuThroughput();

            PCIeReceive = throughput.PCIeReceive;
            PCIeTrasmit = throughput.PCIeTrasmit;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
