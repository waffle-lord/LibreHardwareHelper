﻿using LibreHardware_Helper;
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
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                Console.SetWindowSize(Console.WindowWidth, 33);

            _consoleHeight = Console.WindowHeight;
            _consoleWidth = Console.WindowWidth;
            _libreHardwareHelper = new LibreHardwareHelper();
        }

        public async Task StartRendering()
        {
            while (true)
            {
                var rootLayout = new Layout("root");

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

                rootLayout.SplitRows(topLayout, bottomLayout);

                // add layouts for updating
                _layouts.Clear();
                _layouts.Add(cpuLayout);
                _layouts.Add(memLayout);
                _layouts.Add(gpuLayout);

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