//Author: Elisabeth Gisser

namespace ModBusHistorian.ViewModels;
public class TargetViewModel
{
	public TargetViewModel(string target)
	{
		Target = target;
	}

	public string Target { get; }
}