//Author: Elisabeth Gisser

using System.ComponentModel.DataAnnotations;

namespace ModBusHistorian.ViewModels;

public class QueryViewModel(
	DateTimeRangeViewModel range,
	int intervalMs,
	TargetViewModel[] targets,
	OutputFormat format,
	int maxDataPoints)
{
	[Required]
	public DateTimeRangeViewModel Range { get; } = range;

	[Required]
	public int IntervalMs { get; } = intervalMs;

	[Required]
	public TargetViewModel[] Targets { get; } = targets;

	[Required]
	public OutputFormat Format { get; } = format;

	[Required]
	public int MaxDataPoints { get; } = maxDataPoints;
}