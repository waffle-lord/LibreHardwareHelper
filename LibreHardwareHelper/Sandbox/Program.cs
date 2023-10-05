// See https://aka.ms/new-console-template for more information

using LibreHardware_Helper;
using Spectre.Console;

var hardwareHelper = new LibreHardwareHelper();

// setup main table
var mainTable = new Table().HideHeaders();
mainTable.AddColumn("Data");
mainTable.Expand();



AnsiConsole.Live(mainTable).Start((ctx) =>
{
    // add columns for data
    var cpuTable = new Table().AddColumn("v1").AddColumn("v2").AddColumn("v3").AddColumn("v4").AddColumn("coreTable").HideHeaders().HorizontalBorder();
    var coreTable = new Table().AddColumn("coreName").AddColumn("data").HideHeaders();

    var memTable = new Table().AddColumn("v1").HideHeaders().HorizontalBorder();

    var gpuTable = new Table().AddColumn("v1").AddColumn("v2").AddColumn("v3").AddColumn("v4").AddColumn("v5").HideHeaders().HorizontalBorder();
    var gpuMemTable = new Table().AddColumn("v1").AddColumn("v2").HideHeaders();

    // set some temp titles
    cpuTable.Title = new TableTitle("Loading Cpu... ");
    coreTable.Title = new TableTitle("Cores");
    memTable.Title = new TableTitle("Loading Mem... ");
    gpuTable.Title = new TableTitle("Loading Gpu... ");
    gpuMemTable.Title = new TableTitle("Gpu Memory");

    // take all the space 
    cpuTable.Expand();
    memTable.Expand();
    gpuTable.Expand();

    // add other tables to main table, some init fancy load in wait as well because why not *shrug
    mainTable.AddRow(cpuTable);
    ctx.Refresh();
    Thread.Sleep(100);

    mainTable.AddRow(memTable);
    ctx.Refresh();
    Thread.Sleep(100);

    mainTable.AddRow(gpuTable);
    ctx.Refresh();
    Thread.Sleep(100);

    // loop forever loading data into tables and refreshing every 2 seconds
    while (true)
    {
        // get hardware data
        var cpu = hardwareHelper.GetCpuData();
        var mem = hardwareHelper.GetMemoryData();
        var gpu = hardwareHelper.GetGpuData();

        if (cpu == null)
        {
            Console.WriteLine("Failed to get CPU");
        }

        if (mem == null)
        {
            Console.WriteLine("Failed to get Memory");
        }

        if (gpu == null)
        {
            Console.WriteLine("Failed to get GPU");
        }

        // set table titles
        cpuTable.Title = new TableTitle(cpu.Name);
        memTable.Title = new TableTitle("Memory");
        gpuTable.Title = new TableTitle($"({gpu.Kind}) {gpu.Name}");

        // load Cpu data
        cpuTable.Rows.Clear();
        coreTable.Rows.Clear();

        foreach (var core in cpu.Cores)
        {
            coreTable.AddRow(core.Name, $"Load: {core.Load.ToString("0.0")}% - Temp: {core.Temp.ToString("0.0")} C");
        }

        cpuTable.AddRow(new Markup("Total Load"), new Markup($"{cpu.Loads.Total.ToString("0.0")}%"), new Markup("Package Temp"), new Markup($"{cpu.Temps.PackageTemp.ToString("0.0")} C"), coreTable);
        cpuTable.AddRow("Bus Speed", $"{cpu.Clocks.BusSpeed} MHz", "", "", $"Core Average Temp: {cpu.Temps.CoreAverage.ToString("0.0")} C");

        // load mem data
        memTable.Rows.Clear();

        memTable.AddRow(new BreakdownChart()
            .ShowPercentage()
            .AddItem("Used", Math.Round(mem.PercentUsed, 2), Color.Blue3)
            .AddItem("Available", Math.Round(mem.PercentAvailable, 2), Color.CornflowerBlue));

        // load gpu data (if it exists)
        if (gpu.Kind == LibreHardware_Helper.Model.HardwareData.GPU.GpuKind.NVIDIA || gpu.Kind == LibreHardware_Helper.Model.HardwareData.GPU.GpuKind.AMD)
        {
            gpuTable.Rows.Clear();
            gpuMemTable.Rows.Clear();

            gpuTable.AddRow(new BreakdownChart()
                .ShowPercentage()
                .AddItem("Used", Math.Round(gpu.Memory.PercentUsed, 2), Color.Blue3)
                .AddItem("Available", Math.Round(gpu.Memory.PercentAvailable, 2), Color.CornflowerBlue));

            gpuTable.AddRow(new Markup("Core Load"), new Markup($"{gpu.Loads.Core.ToString("0.0")}%"), new Markup("Video Engine Load"), new Markup($"{gpu.Loads.VideoEngine.ToString("0.0")}%"), gpuMemTable);
            gpuTable.AddRow("Core Temp", $"{gpu.Temps.Core.ToString("0.0")} C", "Bus Load", $"{gpu.Loads.Bus.ToString("0.0")}%", $"Memory Load: {gpu.Loads.Memory.ToString("0.0")}%\nMemory Controller Load: {gpu.Loads.MemoryController.ToString("0.0")}%");
            gpuTable.AddRow("PCIe Trasmit", $"{gpu.Throughput.PCIeTrasmit.ToString("0.0")}", "PCIe Receive", $"{gpu.Throughput.PCIeReceive.ToString("0.0")}");
        }

        ctx.Refresh();
        Thread.Sleep(2000);
    }
});