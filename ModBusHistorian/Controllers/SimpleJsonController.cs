//Author: Elisabeth Gisser

using ModBusHistorian.Repositories;
using ModBusHistorian.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ModBusHistorian.Controllers;

/// <summary>
/// Implements Grafana Simple JSON interface, see https://grafana.com/grafana/plugins/grafana-simple-json-datasource/
/// </summary>
[ApiController]
[Route("")]
public class SimpleJsonController : ControllerBase
{
    private readonly IDataSeriesRepository _repository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="repository"></param>
    public SimpleJsonController(IDataSeriesRepository repository)
    {
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
        var referenceViewModels = references.Select(r => new ReferenceViewModel { Text = r.Name }).ToList();
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
            var dataPoints = await _repository.GetDataPointsAsync(target.Target, query.Range.From, query.Range.To, query.MaxDataPoints);

            var timeSeriesData = dataPoints.Select(dp => new[] { dp.Value, dp.DateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds }).ToArray();

            resultList.Add(new TimeSeriesViewModel(target.Target, timeSeriesData!));
        }

        return Ok(resultList);
    }
}