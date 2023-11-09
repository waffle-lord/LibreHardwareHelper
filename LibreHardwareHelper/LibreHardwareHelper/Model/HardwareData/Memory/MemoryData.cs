using System;
using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.Memory;

public class MemoryData : PropertyNotifierBase
{
    private readonly LibreHardwareHelper _helper;
    private float _amountAvailable;

    private float _amountUsed;

    private float _percentUsed;

    private float _virtualAmountAvailable;

    private float _virtualAmountUsed;

    private float _virtualPercentUsed;

    public MemoryData(IHardware memory, LibreHardwareHelper helper)
    {
        if (memory.HardwareType != HardwareType.Memory) return;

        _helper = helper;

        foreach (var s in memory.Sensors)
            switch (s.Name)
            {
                case "Memory":
                {
                    PercentUsed = s.Value ?? 0;
                    break;
                }
                case "Memory Used":
                {
                    AmountUsed = s.Value ?? 0;
                    break;
                }
                case "Memory Available":
                {
                    AmountAvailable = s.Value ?? 0;
                    break;
                }
                case "Virtual Memory":
                {
                    VirtualPercentUsed = s.Value ?? 0;
                    break;
                }
                case "Virtual Memory Used":
                {
                    VirtualAmountUsed = s.Value ?? 0;
                    break;
                }
                case "Virtual Memory Available":
                {
                    VirtualAmountAvailable = s.Value ?? 0;
                    break;
                }
            }
    }

    public float PercentUsed
    {
        get => _percentUsed;
        private set
        {
            if (_percentUsed != value)
            {
                _percentUsed = value;
                RaisePropertyChanged(nameof(PercentUsed));
                RaisePropertyChanged(nameof(PercentAvailable));
            }
        }
    }

    public float AmountUsed
    {
        get => _amountUsed;
        private set
        {
            if (_amountUsed != value)
            {
                _amountUsed = value;
                RaisePropertyChanged(nameof(AmountUsed));
                RaisePropertyChanged(nameof(Total));
            }
        }
    }

    public float AmountAvailable
    {
        get => _amountAvailable;
        private set => RaiseAndSetIfChanged(ref _amountAvailable, value);
    }

    public float VirtualPercentUsed
    {
        get => _virtualPercentUsed;
        private set
        {
            if (_virtualPercentUsed != value)
            {
                _virtualPercentUsed = value;
                RaisePropertyChanged(nameof(VirtualPercentUsed));
                RaisePropertyChanged(nameof(VirtualPercentAvailable));
            }
        }
    }

    public float VirtualAmountUsed
    {
        get => _virtualAmountUsed;
        private set
        {
            if (_virtualAmountUsed != value)
            {
                _virtualAmountUsed = value;
                RaisePropertyChanged(nameof(VirtualAmountUsed));
                RaisePropertyChanged(nameof(VirtualTotal));
            }
        }
    }

    public float VirtualAmountAvailable
    {
        get => _virtualAmountAvailable;
        private set => RaiseAndSetIfChanged(ref _virtualAmountAvailable, value);
    }

    public float Total => AmountUsed + AmountAvailable;
    public float VirtualTotal => VirtualAmountUsed + VirtualAmountAvailable;

    public float PercentAvailable => 100 - PercentUsed;
    public float VirtualPercentAvailable => 100 - VirtualPercentUsed;

    /// <summary>
    ///     Update this <see cref="MemoryData" /> objects data
    /// </summary>
    public void Update()
    {
        var memData = _helper.GetMemoryData();

        PercentUsed = memData.PercentUsed;
        AmountUsed = memData.AmountUsed;
        AmountAvailable = memData.AmountAvailable;

        VirtualPercentUsed = memData.VirtualPercentUsed;
        VirtualAmountUsed = memData.VirtualAmountUsed;
        VirtualAmountAvailable = memData.VirtualAmountAvailable;

        RaiseDataUpdated();
    }

    public event EventHandler DataUpdated;

    protected virtual void RaiseDataUpdated()
    {
        DataUpdated?.Invoke(this, null);
    }
}