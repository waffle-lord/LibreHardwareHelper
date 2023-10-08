using LibreHardware_Helper.Model.HardwareData.CPU;
using Sandbox.Interfaces;
using Spectre.Console;

namespace Sandbox.Display.LayoutManagers
{
    internal class CpuLayout : LayoutManagerBase, IDisplayLayout
    {
        private CpuData? _cpu;
        public Layout Layout { get; private set; }

        public CpuLayout(CpuData cpu)
        {
            _cpu = cpu;
            Layout = new Layout("cpu");
        }

        public void Update()
        {
            if (_cpu == null)
            {
                Layout["cpu"].Update(new Align(new Text("could not get cpu info"), HorizontalAlignment.Center, VerticalAlignment.Middle));
                return;
            }

            _cpu.Update();

            // package info
            var packageInfoGrid = new Grid().AddColumn().AddColumn().AddColumn().AddColumn().AddColumn().AddColumn();

            packageInfoGrid.AddRow();
            packageInfoGrid.AddRow("Total Load", GetPercentColorString(_cpu.Loads.Total), "%", "Bus Speed", $"{_cpu.Clocks.BusSpeed.ToString("0.0")}", "MHz");
            packageInfoGrid.AddRow("Package Temp", GetTempColorString(_cpu.Temps.PackageTemp), "C", "Core Temp Average", GetTempColorString(_cpu.Temps.CoreAverage), "C");

            // core info
            var coresGrid = new Grid().AddColumn().AddColumn().AddColumn().AddColumn().AddColumn().AddColumn().AddColumn();

            coresGrid.AddRow("Core", "Load", " ", "Temp", " ", "Clock", " ");

            foreach ( var core in _cpu.Cores )
            {
                coresGrid.AddRow(
                    $"Core #{core.Number}",
                           GetPercentColorString(core.Load),
                           "%",
                           GetTempColorString(core.Temp),
                           "C",
                           $"[grey]{core.ClockSpeed.ToString("0.0").EscapeMarkup()}[/]",
                           "MHz"
                           );
            }

            Layout["cpu"].Update(new Panel(
                new Grid()
                .AddColumn()
                .AddRow(packageInfoGrid.Expand())
                .AddRow()
                .AddRow(new Rule("Cores").LeftJustified())
                .AddRow(coresGrid.Expand())
                )
                .Header(_cpu.Name).Expand());
        }
    }
}
