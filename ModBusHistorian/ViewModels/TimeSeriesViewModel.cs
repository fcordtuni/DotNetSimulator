//Author: Elisabeth Gisser

using System.Text.Json.Serialization;

namespace ModBusHistorian.ViewModels;

internal struct TimeSeriesViewModel(string target, object[][] dataPoints)
{
	public string Target { get; } = target;

	[JsonPropertyName("datapoints")]
	public object[][] DataPoints { get; } = dataPoints;
}