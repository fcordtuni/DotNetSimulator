//Author: Elisabeth Gisser

using ModBusHistorian.Models;
using ModBusHistorian.Repositories;
using ModBusHistorian.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ModBusHistorian.DataHandling;

namespace ModBusHistorian.Controllers;

/// <summary>
/// Implements Grafana Simple JSON interface, see https://grafana.com/grafana/plugins/grafana-simple-json-datasource/
/// </summary>
[ApiController]
[Route("")]
public class SimpleJsonController : ControllerBase
{
    private readonly ILogger<SimpleJsonController> _logger;
    private readonly IDataSeriesRepository _repository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    public SimpleJsonController(ILogger<SimpleJsonController> logger, IDataSeriesRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    /// <summary>
    /// Return 200 ok. Used for "Test connection" on the datasource config page.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Get() => Ok();

    /// <summary>
    /// Used by the find metric options on the query tab in panels.
    /// </summary>
    /// <returns></returns>
    [HttpPost("search")]
    public async Task<IActionResult> Search()
    {
        var references = await _repository.GetReferencesAsync();
        var referenceViewModels = references.Select(r => new ReferenceViewModel { Name = r.Name }).ToList();
        return Ok(referenceViewModels);
    }

    /// <summary>
    /// Returns metrics based on input.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost("query")]
    public async Task<IActionResult> Query([FromBody] QueryViewModel query)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var resultList = new List<TimeSeriesViewModel>();

        foreach (var target in query.Targets.Where(t => !string.IsNullOrWhiteSpace(t.Target)))
        {
            var endDateTime = DateTime.UtcNow;
            var startDateTime = endDateTime.Subtract(TimeSpan.FromMilliseconds(query.IntervalMs));
            var dataPoints = await _repository.GetDataPointsAsync(target.Target, startDateTime, endDateTime);

            var timeSeriesData = dataPoints.Select(dp => new object[] { dp.Value, dp.Timestamp.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds }).ToArray();

            resultList.Add(new TimeSeriesViewModel
            {
                Target = target.Target,
                Datapoints = timeSeriesData
            });
        }

        return Ok(resultList);
    }
}