using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.CPU
{
    public class CpuData : INotifyPropertyChanged
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

        public CpuClock Clocks { get; private set; }
        public CpuLoad Loads { get; private set; }
        public CpuPower Power { get; private set; }
        public CpuTemp Temps { get; private set; }

        public ObservableCollection<CpuCore> Cores { get; private set; }

        public CpuData(IHardware cpu, LibreHardwareHelper helper)
        {
            if (cpu == null) return;

            if (cpu.HardwareType != HardwareType.Cpu)
                throw new ArgumentException($"provided arg is not cpu hardware");

            _helper = helper;

            Name = cpu.Name;
            Clocks = new CpuClock(cpu, _helper);
            Loads = new CpuLoad(cpu, _helper);
            Power = new CpuPower(cpu, _helper);
            Temps = new CpuTemp(cpu, _helper);

            ObservableCollection<CpuCore> cores = new ObservableCollection<CpuCore>(_helper.GetCpuCores(cpu, true));

            Cores = cores;
        }

        /// <summary>
        /// Update this <see cref="CpuData"/> objects data.
        /// </summary>
        public void Update()
        {
            Clocks.Update();
            Loads.Update(true);
            Power.Update(true);
            Temps.Update(true);

            CpuCore[] tempCores = _helper.GetCpuCores(null, true);

            for (int x = 0; x < Cores.Count; x++)
            {
                Cores[x].Update(tempCores[x]);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
