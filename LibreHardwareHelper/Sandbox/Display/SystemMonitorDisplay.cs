using LibreHardware_Helper;
using LibreHardware_Helper.Model.HardwareData.GPU;
using Sandbox.Display.LayoutManagers;
using Sandbox.Interfaces;
using Spectre.Console;

namespace Sandbox.Display
{
    internal class SystemMonitorDisplay
    {
        int _consoleHeight;
        int _consoleWidth;
        LibreHardwareHelper _libreHardwareHelper;
        List<IDisplayLayout> _layouts = new List<IDisplayLayout>();

        public SystemMonitorDisplay()
        {
            _consoleHeight = Console.WindowHeight;
            _consoleWidth = Console.WindowWidth;
            _libreHardwareHelper = new LibreHardwareHelper();

            var cpuData = _libreHardwareHelper.GetCpuData();
            var memData = _libreHardwareHelper.GetMemoryData();
            var gpuData = _libreHardwareHelper.GetGpuData();

            if (cpuData != null)
                _layouts.Add(new CpuLayout(cpuData));

            if (memData != null)
                _layouts.Add(new MemLayout(memData));

            if (gpuData != null && (gpuData.Kind == GpuKind.NVIDIA || gpuData.Kind == GpuKind.AMD))
                _layouts.Add(new GpuLayout(gpuData));
        }

        public async Task StartRendering()
        {
            while (true)
            {
                var rootLayout = new Layout("root");

                // add layouts to root
                rootLayout.SplitRows(_layouts.Select(x => x.Layout).ToArray());

                // run live console
                await AnsiConsole.Live(rootLayout).StartAsync(async ctx =>
                {
                    do
                    {
                        // update info every 1 second
                        await Task.Delay(1000);

                        foreach(var layout in _layouts)
                        {
                            layout.Update();
                        }

                        ctx.Refresh();
                    }
                    while (Console.WindowHeight == _consoleHeight && Console.WindowWidth == _consoleWidth);
                });
            }
        }
    }
}
