using LibreHardware_Helper.Model.HardwareData.Memory;
using Sandbox.Interfaces;
using Spectre.Console;

namespace Sandbox.Display.LayoutManagers
{
    internal class MemLayout : LayoutManagerBase, IDisplayLayout
    {
        private MemoryData? _mem;
        public Layout Layout { get; private set; }

        public MemLayout(MemoryData mem)
        {
            _mem = mem;
            Layout = new Layout("mem");
        }

        public void Update()
        {
            if (_mem == null)
            {
                Layout["mem"].Update(new Align(new Text("could not get memory"), HorizontalAlignment.Center, VerticalAlignment.Middle));
                return;
            }

            _mem.Update();

            var memBreakdown = new BreakdownChart()
                .Expand()
                .ShowPercentage()
                .AddItem($"{_mem.AmountUsed.ToString("0.0")} Gb Used", Math.Round(_mem.PercentUsed, 1), GetPercentColor(_mem.PercentUsed))
                .AddItem($"{_mem.AmountAvailable.ToString("0.0")} Gb Available", Math.Round(_mem.PercentAvailable, 1), Color.Grey);

            var virturalMemBreakdown = new BreakdownChart()
                .Expand()
                .ShowPercentage()
                .AddItem($"{_mem.VirtualAmountUsed.ToString("0.0")} Gb Used", Math.Round(_mem.VirtualPercentUsed, 1), GetPercentColor(_mem.VirtualPercentUsed))
                .AddItem($"{_mem.VirtualAmountAvailable.ToString("0.0")} G Available", Math.Round(_mem.VirtualPercentAvailable, 1), Color.Grey);

            var memGrid = new Grid()
                .AddColumn()
                .AddRow() // spacer
                .AddRow(new Rule($"Physical (Total: {_mem.Total.ToString("0.0")} Gb)").LeftJustified())
                .AddRow(memBreakdown)
                .AddRow(new Rule($"Virtual (Total: {_mem.VirtualTotal.ToString("0.0")} Gb)").LeftJustified())
                .AddRow(virturalMemBreakdown);

            Layout["mem"].Update(new Panel(
                memGrid
                ).Header("Memory").Expand());
        }
    }
}
