using ModBusHistorian.Models;
using ModBusHistorian.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ModBusHistorian.Controllers;

/// <summary>
/// REST API for the historian.
/// </summary>
[ApiController]
[Route("[controller]")]
public class HistorianController(ILogger<HistorianController> logger, IDataSeriesRepository repository)
    : ControllerBase
{
    /// <summary>
    /// Get all references.
    /// </summary>
    /// <returns></returns>
    [HttpGet("references")]
    public async Task<IEnumerable<Reference>> GetReferences()
    {
        return await repository.GetReferencesAsync();
    }
    
    /// <summary>
    /// Get data points for a reference by filter options
    /// </summary>
    /// <param name="reference"></param>
    /// <param name="seconds">Amount of seconds data points are returned</param>
    /// <returns></returns>
    [HttpGet("dataPoints/{reference}/{seconds:int}")]
    public async Task<IEnumerable<DataPoint>> GetDataPoints(string reference, int seconds)
    {
        var endDateTime = DateTime.UtcNow;
        var startDateTime = endDateTime.Subtract(new TimeSpan(0, 0, seconds));
        logger.LogTrace("something to do");
        return await repository.GetDataPointsAsync(reference, startDateTime, endDateTime);
    }
}