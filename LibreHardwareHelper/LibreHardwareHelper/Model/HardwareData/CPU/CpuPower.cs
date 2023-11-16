using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.CPU;

public class CpuPower : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;
    private float _cores;

    private float _graphics;

    private float _memory;

    private float _package;

    public CpuPower(IHardware cpu, LibreHardwareHelper helper)
    {
        if (cpu == null) return;

        if (cpu.HardwareType != HardwareType.Cpu) return;

        _helper = helper;

        foreach (var s in cpu.Sensors)
            switch (s.Name)
            {
                case "Package":
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

    public float Package
    {
        get => _package;
        private set => RaiseAndSetIfChanged(ref _package, value);
    }

    public float Cores
    {
        get => _cores;
        private set => RaiseAndSetIfChanged(ref _cores, value);
    }

    public float Graphics
    {
        get => _graphics;
        private set => RaiseAndSetIfChanged(ref _graphics, value);
    }

    public float Memory
    {
        get => _memory;
        private set => RaiseAndSetIfChanged(ref _memory, value);
    }

    /// <summary>
    ///     Update this <see cref="CpuPower" /> objects data.
    /// </summary>
    /// <param name="dontQueryHardware">Update the values of this object if they differ, but don't ask the hardware to update</param>
    public void Update(bool dontQueryHardware = false)
    {
        var tempPower = _helper.GetCpuPower(null, dontQueryHardware);

        Package = tempPower.Package;
        Cores = tempPower.Cores;
        Graphics = tempPower.Graphics;
        Memory = tempPower.Memory;
    }
}