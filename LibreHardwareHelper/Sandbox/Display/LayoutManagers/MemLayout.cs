using LibreHardware_Helper.Model.HardwareData.Memory;
using Sandbox.Interfaces;
using Spectre.Console;

namespace Sandbox.Display.LayoutManagers
{
    internal class MemLayout : LayoutManagerBase, IDisplayLayout
    {
        private MemoryData _mem;
        public Layout Layout { get; private set; }

        public MemLayout(MemoryData mem)
        {
            _mem = mem;
            Layout = new Layout("mem");
        }

        public void Update()
        {
            _mem.Update();

            Layout["mem"].Update(new Panel(
                new BreakdownChart()
                .Expand()
                .ShowPercentage()
                .AddItem("Used", Math.Round(_mem.PercentUsed, 1), GetPercentColor(_mem.PercentUsed))
                .AddItem("Available", Math.Round(_mem.PercentAvailable, 1), Color.Grey)
                ).Header("Memory"));
        }
    }
}
