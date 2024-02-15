//Author: Elisabeth Gisser

using Microsoft.AspNetCore.Http.HttpResults;

namespace ModBusHistorian.ViewModels;
public class TargetViewModel(string target)
{
	protected TargetViewModel() : this("")
	{
	}

	public string Target { get; } = target;
}