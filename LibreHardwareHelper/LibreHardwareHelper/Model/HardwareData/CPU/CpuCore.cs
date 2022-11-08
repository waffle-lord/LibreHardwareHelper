using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.CPU
{
    /// <summary>
    /// All data relevent to a single CPU core
    /// </summary>
    public class CpuCore : INotifyPropertyChanged
    {
        private int _Number;
        public int Number
        {
            get => _Number;
            private set
            {
                if (_Number != value)
                {
                    _Number = value;
                    RaisePropertyChange(nameof(Number));
                }
            }
        }

        private string _Name;
        public string Name
        {
            get => _Name;
            private set
            {
                if (_Name != value)
                {
                    _Name = value;
                    RaisePropertyChange(nameof(Name));
                }
            }
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
                    RaisePropertyChange(nameof(Temp));
                    RaisePropertyChange(nameof(TempPercentage));
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
            private set
            {
                if (_Load != value)
                {
                    _Load = value;
                    RaisePropertyChange(nameof(Load));
                }
            }
        }

        private float _ClockSpeed;
        public float ClockSpeed
        {
            get => _ClockSpeed;
            set
            {
                if (_ClockSpeed != value)
                {
                    _ClockSpeed = value;
                    RaisePropertyChange(nameof(ClockSpeed));
                }
            }
        }

        private float _TjMaxDistance;
        public float TjMaxDistance
        {
            get => _TjMaxDistance;
            set
            {
                if (_TjMaxDistance != value)
                {
                    _TjMaxDistance = value;
                    RaisePropertyChange(nameof(TjMaxDistance));
                }
            }
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChange(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
