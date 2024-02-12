//Author: FCORDT

using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using ModBusHistorian.Models;

namespace ModBusHistorian.Repositories;

public class InfluxDbDataSeriesRepository(IConfiguration configuration) : IDataSeriesRepository
{
    private readonly InfluxDBClient _client = new(configuration.GetValue<string>("InfluxDB:URL") ?? string.Empty, 
        configuration.GetValue<string>("InfluxDB:Token"));
    private readonly string? _bucket = configuration.GetValue<string>("InfluxDB:Bucket");
    private readonly string? _org = configuration.GetValue<string>("InfluxDB:Organisation");
    private readonly string _tag = configuration.GetValue<string>("InfluxDB:Data-Tag") ?? "default-tag";

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
        GC.SuppressFinalize(this);
        _client.Dispose();
    }

    public void Add(Reference reference, DateTime date, object? value)
    {
        Write(api =>
        {
            var point = PointData
                .Measurement(reference.Name)
                .Field("data", value)
                .Timestamp(date, WritePrecision.Ms)
                .Tag("source", _tag);
            api.WritePoint(point, _bucket, _org);
        });
    }

    public Task<IEnumerable<Reference>> GetReferencesAsync(int? skip = null, int? take = null)
    {
        return QueryAsync(async query =>
        {
            var flux = $"from(bucket: \"{_bucket}\") " +
                       $"|> range(start: 0) " +
                       $"|> filter(fn: (r) => r.source == \"{_tag}\") " +
                       $"|> group(columns: [\"_measurement\"])\n  " +
                       $"|> distinct(column: \"_measurement\")\n  " +
                       $"|> keep(columns: [\"_measurement\"])";
            var tables = await query.QueryAsync(flux, _org);
            return tables.SelectMany(table =>
                table.Records.Select(record =>
                    new Reference(record.GetMeasurement())
                )
            );
        });
    }

    public Task<IEnumerable<DataPoint>> GetDataPointsAsync(Reference reference, DateTime startDateTime, DateTime endDateTime, int? skip = null,
        int? take = null)
    {
        return QueryAsync(async query =>
        {
            var flux = $"from(bucket:\"{_bucket}\") " +
                       $"|> range(start: {startDateTime:O}, stop: {endDateTime:O}) " +
                       $"|> filter(fn: (r) => r._measurement == \"{reference.Name}\") ";
            var tables = await query.QueryAsync(flux, _org);
            return tables.SelectMany(table =>
                table.Records.Select(record =>
                    new DataPoint(reference, record.GetTimeInDateTime()!.Value, record.GetValue())
                )
            );
        });
    }
}