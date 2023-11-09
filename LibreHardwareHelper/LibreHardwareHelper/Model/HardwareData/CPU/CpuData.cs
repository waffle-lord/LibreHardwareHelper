using System;
using System.Collections.ObjectModel;
using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.CPU;

public class CpuData : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;

    private string _name;

    public CpuData(IHardware cpu, LibreHardwareHelper helper)
    {
        if (cpu == null) return;

        if (cpu.HardwareType != HardwareType.Cpu)
            throw new ArgumentException("provided arg is not cpu hardware");

        _helper = helper;

        Name = cpu.Name;
        Clocks = new CpuClock(cpu, _helper);
        Loads = new CpuLoad(cpu, _helper);
        Power = new CpuPower(cpu, _helper);
        Temps = new CpuTemp(cpu, _helper);

        var cores = new ObservableCollection<CpuCore>(_helper.GetCpuCores(cpu, true));

        Cores = cores;
    }

    public string Name
    {
        get => _name;
        private set => RaiseAndSetIfChanged(ref _name, value);
    }

    public CpuClock Clocks { get; }
    public CpuLoad Loads { get; }
    public CpuPower Power { get; }
    public CpuTemp Temps { get; }

    public ObservableCollection<CpuCore> Cores { get; }

    /// <summary>
    ///     Update this <see cref="CpuData" /> objects data.
    /// </summary>
    public void Update()
    {
        Clocks.Update();
        Loads.Update(true);
        Power.Update(true);
        Temps.Update(true);

        var tempCores = _helper.GetCpuCores(null, true);

        for (var x = 0; x < Cores.Count; x++) Cores[x].Update(tempCores[x]);
    }
}