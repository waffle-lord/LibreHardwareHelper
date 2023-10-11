using LibreHardware_Helper;
using Sandbox.Display.LayoutManagers;
using Sandbox.Interfaces;
using Spectre.Console;

namespace Sandbox.Display
{
    internal class SystemMonitorDisplay
    {
        int _consoleHeight;
        int _consoleWidth;
        private readonly LibreHardwareHelper _libreHardwareHelper;
        private readonly List<IDisplayLayout> _layouts = new();

        public SystemMonitorDisplay()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                Console.SetWindowSize(Console.WindowWidth, 33);

            _libreHardwareHelper = new LibreHardwareHelper();
        }

        public async Task StartRendering()
        {
            while (true)
            {
                // save console size to handle resizing
                _layouts.Clear();
                _consoleHeight = Console.WindowHeight;
                _consoleWidth = Console.WindowWidth;

                // get initial data
                var cpuData = _libreHardwareHelper.GetCpuData();
                var memData = _libreHardwareHelper.GetMemoryData();
                var gpuData = _libreHardwareHelper.GetGpuData();

                // setup layout
                var cpuLayout = new CpuLayout(cpuData);
                var memLayout = new MemLayout(memData);
                var gpuLayout = new GpuLayout(gpuData);

                var diskLayout = new Layout("disk");
                var netLayout = new Layout("net");

                var topLayout = new Layout("top")
                    .Ratio(2)
                    .SplitColumns(
                    cpuLayout.Layout,
                    new Layout("topRight").SplitRows(
                        memLayout.Layout.Size(10),
                        diskLayout
                    ));

                var bottomLayout = new Layout("bottom")
                    .SplitColumns(
                    netLayout,
                    gpuLayout.Layout.Ratio(2)
                    );

                var rootLayout = new Layout("root");

                rootLayout.SplitRows(topLayout, bottomLayout);

                // save layouts for updating
                _layouts.Add(cpuLayout);
                _layouts.Add(memLayout);
                _layouts.Add(gpuLayout);

                // run live console
                Console.Clear();
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
