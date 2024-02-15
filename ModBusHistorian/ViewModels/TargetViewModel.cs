//Author: Elisabeth Gisser

namespace ModBusHistorian.ViewModels;
public abstract class TargetViewModel(string target)
{
	public string Target { get; } = target;
}