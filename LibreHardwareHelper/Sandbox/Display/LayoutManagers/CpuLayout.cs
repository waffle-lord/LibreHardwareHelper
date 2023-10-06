using LibreHardware_Helper.Model.HardwareData.CPU;
using Sandbox.Interfaces;
using Spectre.Console;

namespace Sandbox.Display.LayoutManagers
{
    internal class CpuLayout : LayoutManagerBase, IDisplayLayout
    {
        private CpuData _cpu;
        public Layout Layout { get; private set; }

        public CpuLayout(CpuData cpu)
        {
            _cpu = cpu;
            Layout = new Layout("cpu")
                .SplitColumns(
                    new Layout("package").Ratio(2),
                    new Layout("cores")
                );
        }

        public void Update()
        {
            _cpu.Update();

            // package info
            var packageGrid = new Grid().AddColumn().AddColumn();

            packageGrid.AddRow("Total Load", GetPercentColorString(_cpu.Loads.Total));
            packageGrid.AddRow("Package Temp", GetTempColor(_cpu.Temps.PackageTemp));
            packageGrid.AddRow("Bus Speed", $"{_cpu.Clocks.BusSpeed.ToString("0.0")} MHz");


            Layout["cpu"]["package"].Update(new Panel(
                packageGrid
                ).Header(_cpu.Name).Expand());

            // core info
            var coresGrid = new Grid().AddColumn().AddColumn().AddColumn();

            coresGrid.AddRow("Name", "Load", "Temp");

            foreach ( var core in _cpu.Cores )
            {
                coresGrid.AddRow(core.Name, GetPercentColorString(core.Load), GetTempColor(core.Temp));
            }

            Layout["cpu"]["cores"].Update(new Panel(
                coresGrid
                )
                .Header("Cores").Expand());
        }
    }
}
