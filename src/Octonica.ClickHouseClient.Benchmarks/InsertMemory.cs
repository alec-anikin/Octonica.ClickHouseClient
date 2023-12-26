using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Octonica.ClickHouseClient.Benchmarks;

[MemoryDiagnoser]
public class InsertMemory
{
    private ClickHouseConnectionSettings _connectionSettings;

    private List<int[]> _array;
    private List<Memory<int>> _memory;
    private List<ReadOnlyMemory<int>> _readOnlyMemory;

    private const int RowCount = 1000;

    [GlobalSetup]
    public void Setup()
    {
        _connectionSettings = ConnectionSettingsHelper.GetConnectionSettings();

        using (var connection = new ClickHouseConnection(_connectionSettings))
        {
            connection.Open();

            var cmd = connection.CreateCommand("DROP TABLE IF EXISTS MemoryBenchmarks");
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                "CREATE TABLE MemoryBenchmarks(id Array(Int32)) ENGINE = Memory";
            cmd.ExecuteNonQuery();
        }

        _array = new List<int[]>(RowCount);
        _memory = new List<Memory<int>>(RowCount);
        _readOnlyMemory = new List<ReadOnlyMemory<int>>(RowCount);

        for (int i = 0; i < RowCount; i++)
        {
            var array = Enumerable.Range(0, 30).ToArray();

            _array.Add(array);
            _memory.Add(array);
            _readOnlyMemory.Add(array);
        }
    }

    [Benchmark]
    public async Task Array()
    {
        await using var connection = new ClickHouseConnection(_connectionSettings);
        await connection.OpenAsync();

        await using var writer = connection.CreateColumnWriter("INSERT INTO MemoryBenchmarks VALUES");

        await writer.WriteTableAsync(new object[] { _array }, RowCount, CancellationToken.None);
    }

    [Benchmark]
    public async Task Memory()
    {
        await using var connection = new ClickHouseConnection(_connectionSettings);
        await connection.OpenAsync();

        await using var writer = connection.CreateColumnWriter("INSERT INTO MemoryBenchmarks VALUES");

        await writer.WriteTableAsync(new object[] { _memory }, RowCount, CancellationToken.None);
    }

    [Benchmark]
    public async Task ReadonlyMemory()
    {
        await using var connection = new ClickHouseConnection(_connectionSettings);
        await connection.OpenAsync();

        await using var writer = connection.CreateColumnWriter("INSERT INTO MemoryBenchmarks VALUES");

        await writer.WriteTableAsync(new object[] { _readOnlyMemory }, RowCount, CancellationToken.None);
    }
}