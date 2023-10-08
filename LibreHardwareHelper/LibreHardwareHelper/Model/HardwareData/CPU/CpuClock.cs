using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.CPU
{
    public class CpuClock : PropertyNotifierBase
    {
        private LibreHardwareHelper _helper;

        private float _BusSpeed;
        public float BusSpeed
        {
            get => _BusSpeed;
            private set => RaiseAndSetIfChanged(ref _BusSpeed, value);
        }

        public CpuClock(IHardware cpu, LibreHardwareHelper helper)
        {
            if (cpu == null) return;

            if (cpu.HardwareType != HardwareType.Cpu) return;

            _helper = helper;

            foreach (ISensor s in cpu.Sensors)
            {
                if (s.SensorType != SensorType.Clock) continue;

                if (s.Name == "Bus Speed")
                {
                    BusSpeed = s.Value ?? 0;
                    continue;
                }
            }
        }

        /// <summary>
        /// Update this <see cref="CpuClock"/> objects data.
        /// </summary>
        /// <param name="DontQueryHardware">Update the values of this object if they differ, but don't ask the hardware to update</param>
        public void Update(bool DontQueryHardware = false)
        {
            CpuClock tempClocks = _helper.GetCpuClock(null, DontQueryHardware);

            BusSpeed = tempClocks.BusSpeed;
        }
    }
}
