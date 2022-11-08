using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.GPU
{
    public class GpuPower : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
