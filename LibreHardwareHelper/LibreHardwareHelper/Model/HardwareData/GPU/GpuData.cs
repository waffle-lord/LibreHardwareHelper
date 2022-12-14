using LibreHardwareMonitor.Hardware;
using System;
using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.GPU
{
    /* Example data (<> is done)
    <>/gpu-nvidia/0/temperature/0 :: GPU Core - 49
    <>/gpu-nvidia/0/clock/0       :: GPU Core - 1410
    <>/gpu-nvidia/0/clock/1       :: GPU Memory - 7000.98
    <>/gpu-nvidia/0/clock/2       :: GPU Shader - 0
    <>/gpu-nvidia/0/clock/3       :: GPU Video - 0
    <>/gpu-nvidia/0/load/0        :: GPU Core - 4
    <>/gpu-nvidia/0/load/1        :: GPU Memory Controller - 1
    <>/gpu-nvidia/0/load/2        :: GPU Video Engine - 0
    <>/gpu-nvidia/0/load/3        :: GPU Bus - 0
    <>/gpu-nvidia/0/load/4        :: GPU Memory - 18.602753
    /gpu-nvidia/0/fan/0         :: GPU Fan - 0
    /gpu-nvidia/0/control/0     :: GPU Fan - 0
    /gpu-nvidia/0/load/5        :: GPU Power - 22.81
    /gpu-nvidia/0/load/6        :: GPU Board Power - 22.49
    <>/gpu-nvidia/0/smalldata/3   :: D3D Dedicated Memory Used - 1371.4297
    <>/gpu-nvidia/0/smalldata/4   :: D3D Shared Memory Used - 86.51953
    /gpu-nvidia/0/load/7        :: D3D 3D - 0
    /gpu-nvidia/0/load/11       :: D3D Overlay - 0
    /gpu-nvidia/0/load/13       :: D3D Video Decode - 0
    /gpu-nvidia/0/load/8        :: D3D Copy - 0
    /gpu-nvidia/0/load/9        :: D3D Copy - 0
    /gpu-nvidia/0/load/10       :: D3D Copy - 0
    /gpu-nvidia/0/load/12       :: D3D Security - 0
    /gpu-nvidia/0/load/14       :: D3D Video Encode - 0
    /gpu-nvidia/0/load/15       :: D3D VR - 0
    <>/gpu-nvidia/0/smalldata/2   :: GPU Memory Total - 8192
    <>/gpu-nvidia/0/smalldata/0   :: GPU Memory Free - 6668
    <>/gpu-nvidia/0/smalldata/1   :: GPU Memory Used - 1523
    /gpu-nvidia/0/power/0       :: GPU Package - 42.199
    <>/gpu-nvidia/0/throughput/0  :: GPU PCIe Rx - 1024000
    <>/gpu-nvidia/0/throughput/1  :: GPU PCIe Tx - 1024000
    */

    public class GpuData : INotifyPropertyChanged
    {
        private LibreHardwareHelper _helper;

        private string _Name;
        public string Name
        {
            get => _Name;
            private set
            {
                if (_Name != value)
                {
                    _Name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        private GpuKind _Kind;
        public GpuKind Kind => _Kind;

        public GpuLoad Loads { get; private set; }
        public GpuTemp Temps { get; private set; }
        public GpuClock Clocks { get; private set; }
        public GpuFan Fans { get; private set; }
        public GpuMemory Memory { get; private set; }
        public GpuPower Power { get; private set; }
        public GpuThroughput Throughput { get; private set; }

        public GpuData(IHardware gpu, GpuKind kind, LibreHardwareHelper helper)
        {
            if (gpu == null)
            {
                Name = "No GPU Found";
                _Kind = kind;
                return;
            }

            if (gpu.HardwareType != HardwareType.GpuNvidia && gpu.HardwareType != HardwareType.GpuAmd)
                throw new ArgumentException($"provided arg is not supported gpu hardware");

            _helper = helper;

            Name = gpu.Name;

            _Kind = kind;

            Throughput = new GpuThroughput(gpu, _helper);
            Temps = new GpuTemp(gpu, _helper);
            Clocks = new GpuClock(gpu, _helper);
            Memory = new GpuMemory(gpu, _helper);
            Loads = new GpuLoad(gpu, _helper);
        }

        public void Update()
        {
            Throughput.Update();
            Temps.Update();
            Clocks.Update();
            Memory.Update();
            Loads.Update();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
