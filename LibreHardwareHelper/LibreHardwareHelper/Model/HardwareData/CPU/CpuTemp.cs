using LibreHardwareMonitor.Hardware;

namespace LibreHardware_Helper.Model.HardwareData.CPU
{
	public class CpuTemp : PropertyNotifierBase
	{
		private LibreHardwareHelper _helper;

		private float _CoreAverge;

		public float CoreAverage
		{
			get => _CoreAverge;
			private set => RaiseAndSetIfChanged(ref _CoreAverge, value);
		}

		private float _PackageTemp;

		public float PackageTemp
		{
			get => _PackageTemp;
			private set => RaiseAndSetIfChanged(ref _PackageTemp, value);
		}

		private float _CoreMaxTemp;

		public float CoreMaxTemp
		{
			get => _CoreMaxTemp;
			private set => RaiseAndSetIfChanged(ref _CoreMaxTemp, value);
		}

		public CpuTemp(IHardware cpu, LibreHardwareHelper helper)
		{
			if (cpu == null) return;

			if (cpu.HardwareType != HardwareType.Cpu) return;

			_helper = helper;

			foreach (ISensor s in cpu.Sensors)
			{
				if (s.SensorType != SensorType.Temperature) continue;

				switch (s.Name)
				{
					case "CPU Package":
					{
						//add pacakge temp
						PackageTemp = s.Value ?? 0;
						break;
					}
					case "Core Max":
					{
						//add max temp
						CoreMaxTemp = s.Value ?? 0;
						break;
					}
					case "Core Average":
					{
						//add cpu average temp
						CoreAverage = s.Value ?? 0;
						break;
					}
					case "Core (Tctl/Tdie)":
					{
						CoreAverage = s.Value ?? 0;
						break;
					}
					case "CCD1 (Tdie)":
					{
						PackageTemp = s.Value ?? 0;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Update this <see cref="CpuTemp"/> objects data.
		/// </summary>
		/// <param name="DontQueryHardware">Update the values of this object if they differ, but don't ask the hardware to update</param>
		public void Update(bool DontQueryHardware = false)
		{
			CpuTemp tempTemps = _helper.GetCpuTemp(null, DontQueryHardware);

			CoreAverage = tempTemps.CoreAverage;
			PackageTemp = tempTemps.PackageTemp;
			CoreMaxTemp = tempTemps.CoreMaxTemp;
		}
	}
}