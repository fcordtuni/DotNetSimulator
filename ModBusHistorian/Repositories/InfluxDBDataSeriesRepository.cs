using InfluxDB.Client;
using ModBusHistorian.Models;

namespace ModBusHistorian.Repositories;

public class InfluxDBDataSeriesRepository :  IDataSeriesRepository
{
    private readonly InfluxDBClient _client;

    public InfluxDBDataSeriesRepository(IConfiguration configuration)
    {
        _client = new InfluxDBClient(configuration.GetValue<string>("InfluxDB:Token") ?? string.Empty,
            configuration.GetValue<string>("InfluxDB:URL") ?? string.Empty);
    }
    
    private void Write(Action<WriteApi> action)
    {
        using var write = _client.GetWriteApi();
        action(write);
    }

    private async Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
    {
        var query = _client.GetQueryApi();
        return await action(query);
    }
    
    public void Dispose()
    {
        _client.Dispose();
    }

    public void Add(Reference reference, DateTime date, object? value)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Reference>> GetReferencesAsync(int? skip = null, int? take = null)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DataPoint>> GetDataPointsAsync(Reference reference, DateTime startDateTime, DateTime endDateTime, int? skip = null,
        int? take = null)
    {
        throw new NotImplementedException();
    }
}