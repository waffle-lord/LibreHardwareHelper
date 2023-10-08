#if MONITOR || RELEASE
using Sandbox.Display;

// show monitor
var monitor = new SystemMonitorDisplay();

await monitor.StartRendering();

#else
using LibreHardwareMonitor.Hardware;

// dump sensor data
var comp = new Computer()
{
    IsBatteryEnabled = true,
    IsControllerEnabled = true,
    IsCpuEnabled = true,
    IsGpuEnabled = true,
    IsMemoryEnabled = true,
    IsNetworkEnabled = true,
    IsMotherboardEnabled = true,
    IsPsuEnabled = true,
    IsStorageEnabled = true
};

comp.Open();

do
{
    Console.Clear();

    foreach (var hardware in comp.Hardware)
    {
        hardware.Update();

        Console.WriteLine("");
        Console.WriteLine(hardware.Name);
        foreach (var sensor in hardware.Sensors)
        {
            Console.WriteLine($" - {sensor.SensorType}::{sensor.Name} - {sensor.Value}");
        }
    }

    Console.WriteLine();
    Console.WriteLine("Press q to quit, any other key to refresh ...");
}
while (Console.ReadKey().KeyChar != 'q');
#endif