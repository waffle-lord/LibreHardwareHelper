using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.CPU;

public class CpuPower : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;
    private float _Cores;

    private float _Graphics;

    private float _Memory;

    private float _Package;

    public CpuPower(IHardware cpu, LibreHardwareHelper helper)
    {
        if (cpu == null) return;

        if (cpu.HardwareType != HardwareType.Cpu) return;

        _helper = helper;

        foreach (var s in cpu.Sensors)
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

    public float Package
    {
        get => _Package;
        private set => RaiseAndSetIfChanged(ref _Package, value);
    }

    public float Cores
    {
        get => _Cores;
        private set => RaiseAndSetIfChanged(ref _Cores, value);
    }

    public float Graphics
    {
        get => _Graphics;
        private set => RaiseAndSetIfChanged(ref _Graphics, value);
    }

    public float Memory
    {
        get => _Memory;
        private set => RaiseAndSetIfChanged(ref _Memory, value);
    }

    /// <summary>
    ///     Update this <see cref="CpuPower" /> objects data.
    /// </summary>
    /// <param name="DontQueryHardware">Update the values of this object if they differ, but don't ask the hardware to update</param>
    public void Update(bool DontQueryHardware = false)
    {
        var tempPower = _helper.GetCpuPower(null, DontQueryHardware);

        Package = tempPower.Package;
        Cores = tempPower.Cores;
        Graphics = tempPower.Graphics;
        Memory = tempPower.Memory;
    }
}