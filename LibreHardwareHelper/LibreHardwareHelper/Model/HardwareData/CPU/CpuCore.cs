namespace LibreHardware_Helper.Model.HardwareData.CPU
{
    /// <summary>
    /// All data relevent to a single CPU core
    /// </summary>
    public class CpuCore : PropertyNotifierBase
    {
        private int _Number;
        public int Number
        {
            get => _Number;
            private set => RaiseAndSetIfChanged(ref _Number, value);
        }

        private string _Name;
        public string Name
        {
            get => _Name;
            private set => RaiseAndSetIfChanged(ref _Name, value);
        }

        private float _Temp;
        public float Temp
        {
            get => _Temp;
            private set
            {
                if (_Temp != value)
                {
                    _Temp = value;
                    RaisePropertyChanged(nameof(Temp));
                    RaisePropertyChanged(nameof(TempPercentage));
                }
            }
        }

        public float TempPercentage
        {
            get
            {
                float maxTemp = Temp + TjMaxDistance;

                return Temp / maxTemp * 100;
            }
        }

        private float _Load;
        public float Load
        {
            get => _Load;
            private set => RaiseAndSetIfChanged(ref _Load, value);
        }

        private float _ClockSpeed;
        public float ClockSpeed
        {
            get => _ClockSpeed;
            private set => RaiseAndSetIfChanged(ref _ClockSpeed, value);
        }

        private float _TjMaxDistance;
        public float TjMaxDistance
        {
            get => _TjMaxDistance;
            private set => RaiseAndSetIfChanged(ref _TjMaxDistance, value);
        }

        public float MaxTemp => Temp + TjMaxDistance;

        /// <summary>
        /// Update this <see cref="CpuCore"/> objects data.
        /// </summary>
        /// <param name="DontQueryHardware">Update the values of this object if they differ, but don't ask the hardware to update</param>
        public void Update(CpuCore Core)
        {
            if (Core.Name != Name) return;

            Temp = Core.Temp;
            Load = Core.Load;
            ClockSpeed = Core.ClockSpeed;
            TjMaxDistance = Core.TjMaxDistance;
        }

        public CpuCore(int Number, string Name, float Temp, float Load, float ClockSpeed, float TjMaxDistance)
        {
            this.Number = Number;
            this.Name = Name;
            this.Temp = Temp;
            this.Load = Load;
            this.ClockSpeed = ClockSpeed;
            this.TjMaxDistance = TjMaxDistance;
        }
    }
}
