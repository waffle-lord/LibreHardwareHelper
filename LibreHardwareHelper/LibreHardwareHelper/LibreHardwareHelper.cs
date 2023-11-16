﻿using System;
using System.Collections.Generic;
using System.Linq;
using LibreHardware_Helper.Model.HardwareData.CPU;
using LibreHardware_Helper.Model.HardwareData.GPU;
using LibreHardware_Helper.Model.HardwareData.Memory;
using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper;

public class LibreHardwareHelper : IDisposable
{
    private readonly Computer _comp;
    private GpuKind _gpuKind;

    /// <summary>
    ///     A helper class to get data from librehardware monitor
    /// </summary>
    public LibreHardwareHelper()
    {
        _comp = new Computer
        {
            IsCpuEnabled = true,
            IsMemoryEnabled = true,
            IsGpuEnabled = true
        };

        _comp.Open();
    }

    public void Dispose()
    {
        _comp.Close();
    }

    private IHardware PrepHardware(IHardware hardware, HardwareType hardwareType, bool dontQueryHardware = false)
    {
        if (hardware == null) hardware = _comp.Hardware.FirstOrDefault(x => x.HardwareType == hardwareType);

        if (hardware == null) return null;

        if (!dontQueryHardware) hardware.Update();

        return hardware;
    }

    #region Memory Data

    /// <summary>
    ///     Get Memory Inoformation
    /// </summary>
    /// <param name="memory">Optionally, specify the Memory to get data from</param>
    /// <returns></returns>
    public MemoryData GetMemoryData(IHardware memory = null)
    {
        memory = PrepHardware(memory, HardwareType.Memory);

        return new MemoryData(memory, this);
    }

    #endregion

    #region Cpu Data

    /// <summary>
    ///     Get all CPU related data
    /// </summary>
    /// <param name="cpu">Optionally, specify the CPU to get data from</param>
    /// <returns></returns>
    public CpuData GetCpuData(IHardware cpu = null)
    {
        cpu = PrepHardware(cpu, HardwareType.Cpu);

        return new CpuData(cpu, this);
    }

    /// <summary>
    ///     Get all CPU core specific data
    /// </summary>
    /// <param name="cpu">Optionally, specify the CPU to get data from</param>
    /// <param name="dontQueryHardware">Don't update the hardware values</param>
    /// <returns></returns>
    public CpuCore[] GetCpuCores(IHardware cpu = null, bool dontQueryHardware = false)
    {
        cpu = PrepHardware(cpu, HardwareType.Cpu, dontQueryHardware);

        if (cpu == null) return new CpuCore[0];

        var cores = new List<CpuCore>();

        var coreBuilders = new Dictionary<int, CpuCoreBuilder>();

        var coreRelatedInfo = cpu.Sensors.Where(x => x.Name.ToLower().Contains("core #")).ToArray();

        // seems to be a consistent way to get only physical cores across types
        var coreCount = coreRelatedInfo.Count(x => x.SensorType == SensorType.Clock);

        foreach (var s in coreRelatedInfo)
        {
            // this should skip threads, atleast AMD is after all the cores, index starts at 1
            if (s.Index > coreCount || s.Index == 0) continue;

            CpuCoreBuilder builder;

            if (!coreBuilders.TryGetValue(s.Index, out builder))
            {
                builder = new CpuCoreBuilder().WithNumber(s.Index).WithName(s.Name);
                coreBuilders.Add(s.Index, builder);
            }

            switch (s.SensorType)
            {
                case SensorType.Clock:
                {
                    builder.ClockSpeed = s.Value ?? 0;
                    break;
                }
                case SensorType.Load:
                {
                    builder.Load = s.Value ?? 0;
                    break;
                }
                case SensorType.Temperature:
                {
                    if (s.Name.Contains("TjMax"))
                        builder.TjMaxDistance = s.Value ?? 0;
                    else
                        builder.Temp = s.Value ?? 0;

                    break;
                }
            }

            coreBuilders[s.Index] = builder;
        }

        foreach (var builder in coreBuilders.Values) cores.Add(builder.Build());

        return cores.ToArray();
    }

    /// <summary>
    ///     Get CPU loads
    /// </summary>
    /// <param name="cpu">Optionally, specify the CPU to get data from</param>
    /// <param name="dontQueryHardware">Don't update the hardware values</param>
    /// <returns>Load values are percentage used</returns>
    public CpuLoad GetCpuLoad(IHardware cpu = null, bool dontQueryHardware = false)
    {
        cpu = PrepHardware(cpu, HardwareType.Cpu, dontQueryHardware);

        return new CpuLoad(cpu, this);
    }

    /// <summary>
    ///     Get CPU Temperatures
    /// </summary>
    /// <param name="cpu">Optionally, specify the CPU to get data from</param>
    /// <param name="dontQueryHardware">Don't update the hardware values</param>
    /// <returns></returns>
    /// <remarks>Temps are in Celcius</remarks>
    public CpuTemp GetCpuTemp(IHardware cpu = null, bool dontQueryHardware = false)
    {
        cpu = PrepHardware(cpu, HardwareType.Cpu, dontQueryHardware);

        return new CpuTemp(cpu, this);
    }

    /// <summary>
    ///     Get CPU clock speeds
    /// </summary>
    /// <param name="cpu">Optionally, specify the CPU to get data from</param>
    /// <param name="dontQueryHardware">Don't update the hardware values</param>
    /// <returns>Clock speeds are in Megahertz</returns>
    public CpuClock GetCpuClock(IHardware cpu = null, bool dontQueryHardware = false)
    {
        cpu = PrepHardware(cpu, HardwareType.Cpu, dontQueryHardware);

        return new CpuClock(cpu, this);
    }

    /// <summary>
    ///     Get CPU Power information
    /// </summary>
    /// <param name="cpu">Optionally, specify the CPU to get data from</param>
    /// <param name="dontQueryHardware">Don't update the hardware values</param>
    /// <returns>Power information is in Watts</returns>
    public CpuPower GetCpuPower(IHardware cpu = null, bool dontQueryHardware = false)
    {
        cpu = PrepHardware(cpu, HardwareType.Cpu, dontQueryHardware);

        return new CpuPower(cpu, this);
    }

    #endregion

    #region Gpu Data

    private HardwareType? GetGpuHardwareType()
    {
        switch (_gpuKind)
        {
            case GpuKind.Nvidia:
                return HardwareType.GpuNvidia;
            case GpuKind.Amd:
                return HardwareType.GpuAmd;
            case GpuKind.Intel:
                return HardwareType.GpuIntel;
            default:
                return null;
        }
    }

    /// <summary>
    ///     Get all GPU related data
    /// </summary>
    /// <param name="gpu">Optionally, specify the GPU to get data from</param>
    /// <returns></returns>
    public GpuData GetGpuData(IHardware gpu = null)
    {
        switch (_gpuKind)
        {
            case GpuKind.Unknown:
                gpu = PrepHardware(gpu, HardwareType.GpuNvidia);

                if (gpu != null)
                {
                    _gpuKind = GpuKind.Nvidia;
                    break;
                }

                gpu = PrepHardware(gpu, HardwareType.GpuAmd);

                if (gpu != null)
                {
                    _gpuKind = GpuKind.Amd;
                    break;
                }

                gpu = PrepHardware(gpu, HardwareType.GpuIntel);

                if (gpu != null)
                {
                    _gpuKind = GpuKind.Intel;
                    break;
                }

                _gpuKind = GpuKind.None;

                return new GpuData(null, GpuKind.None, this);
            case GpuKind.None:
                return new GpuData(null, GpuKind.None, this);
            case GpuKind.Nvidia:
                gpu = PrepHardware(gpu, HardwareType.GpuNvidia);
                break;
            case GpuKind.Amd:
                gpu = PrepHardware(gpu, HardwareType.GpuAmd);
                break;
            case GpuKind.Intel:
                gpu = PrepHardware(gpu, HardwareType.GpuIntel);
                break;
        }

        return new GpuData(gpu, _gpuKind, this);
    }

    /// <summary>
    ///     Get GPU Throughput data
    /// </summary>
    /// <param name="gpu">Optionally, specify the gpu to get data from</param>
    /// <param name="dontQueryHardware">Don't update the hardware values</param>
    /// <returns></returns>
    public GpuThroughput GetGpuThroughput(IHardware gpu = null, bool dontQueryHardware = false)
    {
        var gpuType = GetGpuHardwareType();

        if (!gpuType.HasValue) return null;

        gpu = PrepHardware(gpu, gpuType.Value, dontQueryHardware);

        return new GpuThroughput(gpu, this);
    }

    /// <summary>
    ///     Get the GPU temp data
    /// </summary>
    /// <param name="gpu">Optionally, specify the gpu to get data from</param>
    /// <param name="dontQueryHardware">Don't update the hardware values</param>
    /// <returns></returns>
    public GpuTemp GetGpuTemp(IHardware gpu = null, bool dontQueryHardware = false)
    {
        var gpuType = GetGpuHardwareType();

        if (!gpuType.HasValue) return null;

        gpu = PrepHardware(gpu, gpuType.Value, dontQueryHardware);

        return new GpuTemp(gpu, this);
    }

    /// <summary>
    ///     Get the gpu clock data
    /// </summary>
    /// <param name="gpu">Optionally, specify the gpu to get data from</param>
    /// <param name="dontQueryHardware">Don't update the hardware values</param>
    /// <returns></returns>
    public GpuClock GetGpuClock(IHardware gpu = null, bool dontQueryHardware = false)
    {
        var gpuType = GetGpuHardwareType();

        if (!gpuType.HasValue) return null;

        gpu = PrepHardware(gpu, gpuType.Value, dontQueryHardware);

        return new GpuClock(gpu, this);
    }

    /// <summary>
    ///     Get the gpu memory data
    /// </summary>
    /// <param name="gpu">Optionally, specify the gpu to get data from</param>
    /// <param name="dontQueryHardware">Don't update the hardware values</param>
    /// <returns></returns>
    public GpuMemory GetGpuMemory(IHardware gpu = null, bool dontQueryHardware = false)
    {
        var gpuType = GetGpuHardwareType();

        if (!gpuType.HasValue) return null;

        gpu = PrepHardware(gpu, gpuType.Value, dontQueryHardware);

        return new GpuMemory(gpu, this);
    }

    /// <summary>
    ///     Get the gpu load data
    /// </summary>
    /// <param name="gpu">Optionally, specify the gpu to get data from</param>
    /// <param name="dontQueryHardware">Don't update the hardware values</param>
    /// <returns></returns>
    public GpuLoad GetGpuLoad(IHardware gpu = null, bool dontQueryHardware = false)
    {
        var gpuType = GetGpuHardwareType();

        if (!gpuType.HasValue) return null;

        gpu = PrepHardware(gpu, gpuType.Value, dontQueryHardware);

        return new GpuLoad(gpu, this);
    }

    #endregion
}