using LibreHardware_Helper.Model.HardwareData.GPU;
using Sandbox.Interfaces;
using Spectre.Console;

namespace Sandbox.Display.LayoutManagers
{
    internal class GpuLayout : IDisplayLayout
    {
        private GpuData _gpu;
        public Layout Layout { get; private set; }

        public GpuLayout(GpuData gpu)
        {
            _gpu = gpu;
            Layout = new Layout("gpu");
        }

        public void Update()
        {

        }
    }
}
