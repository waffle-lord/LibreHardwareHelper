namespace LibreHardware_Helper.Model.HardwareData.CPU
{
    public class CoreInfo : PropertyNotifierBase
    {
        private LibreHardwareHelper _helper;

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

        private float _Value;
        public float Value
        {
            get => _Value;
            private set => RaiseAndSetIfChanged(ref _Value, value);
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
    }
}
