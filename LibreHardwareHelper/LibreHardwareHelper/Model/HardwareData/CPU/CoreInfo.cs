using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.CPU
{
    public class CoreInfo : INotifyPropertyChanged
    {
        private LibreHardwareHelper _helper;

        private int _Number;
        public int Number
        {
            get => _Number;
            private set
            {
                if (_Number != value)
                {
                    _Number = value;
                    RaisePropertyChanged(nameof(Number));
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
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        private float _Value;
        public float Value
        {
            get => _Value;
            private set
            {
                if (_Value != value)
                {
                    _Value = value;
                    RaisePropertyChanged(nameof(Value));
                }
            }
        }

        /// <summary>
        /// Update this <see cref="CoreInfo"/> objects data.
        /// </summary>
        public void Update(CoreInfo coreInfo)
        {
            Number = coreInfo.Number;
            Name = coreInfo.Name;
            Value = coreInfo.Value;
        }

        public CoreInfo(string Name, float? Value, LibreHardwareHelper helper)
        {
            _helper = helper;
            this.Number = _helper.GetCoreNumber(Name);
            this.Name = Name;
            this.Value = Value ?? 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
