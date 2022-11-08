using System.ComponentModel;

namespace LibreHardware_Helper.Model.HardwareData.GPU
{
    public class GpuFan : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
