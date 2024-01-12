//Author: Elisabeth Gisser

using System.Text.Json.Serialization;

namespace ModBusHistorian.ViewModels;

internal struct TimeSeriesViewModel
{
	public TimeSeriesViewModel(string target, object?[][] dataPoints)
	{
		Target = target;
		DataPoints = dataPoints;
	}

	public string Target { get; }

	[JsonPropertyName("datapoints")]
	public object[][] DataPoints { get; }
}