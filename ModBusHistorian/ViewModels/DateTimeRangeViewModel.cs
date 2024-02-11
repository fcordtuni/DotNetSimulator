//Author: Elisabeth Gisser

namespace ModBusHistorian.ViewModels;

public struct DateTimeRangeViewModel(DateTime from, DateTime to)
{
    public DateTime From { get; set; } = from;
    public DateTime To { get; set; } = to;
}