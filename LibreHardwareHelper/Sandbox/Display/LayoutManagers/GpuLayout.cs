﻿using LibreHardware_Helper.Model.HardwareData.GPU;
using Sandbox.Interfaces;
using Spectre.Console;

namespace Sandbox.Display.LayoutManagers
{
    internal class GpuLayout : LayoutManagerBase, IDisplayLayout
    {
        private GpuData? _gpu;
        public Layout Layout { get; private set; }

        public GpuLayout(GpuData gpu)
        {
            _gpu = gpu;
            Layout = new Layout("gpu");
        }

        private string GetGpuMemString(float mb)
        {
            return mb >= 1024 ? $"{(mb / 1024).ToString("0.0")} Gb" : $"{mb.ToString("0.0")} Mb";
        }

        public void Update()
        {
            if (_gpu == null || _gpu.Kind == GpuKind.Unknown)
            {
                Layout["gpu"].Update(new Align(new Text("No gpu detected"), HorizontalAlignment.Center, VerticalAlignment.Middle));
                return;
            }
                
            _gpu.Update();

            var gpuTable = new Table().AddColumn("Info").AddColumn("Memory").MinimalBorder().Expand();

            var gpuInfoGrid = new Grid()
                .AddColumn()
                .AddColumn()
                .AddColumn()
                .AddRow("Core Load", GetPercentColorString(_gpu.Loads.Core), "%")
                .AddRow("Mem Controller", GetPercentColorString(_gpu.Loads.MemoryController), "%")
                .AddRow("Bus Load", GetPercentColorString(_gpu.Loads.Bus), "%")
                .AddRow("Power Load", GetPercentColorString(_gpu.Loads.Power), "%");

            

            var gpuMemory = new BreakdownChart()
                .ShowPercentage()
                .AddItem($"{GetGpuMemString(_gpu.Memory.AmountUsed)} / {GetGpuMemString(_gpu.Memory.Total)} Used", _gpu.Memory.PercentUsed, GetPercentColor(_gpu.Memory.PercentUsed))
                .AddItem($"{GetGpuMemString(_gpu.Memory.AmountAvailable)} Available", _gpu.Memory.PercentAvailable, Color.Grey);


            gpuTable.AddRow(gpuInfoGrid, gpuMemory);

            Layout["gpu"].Update(new Panel(
                gpuTable
                ).Header(_gpu.Name).Expand());
        }
    }
}