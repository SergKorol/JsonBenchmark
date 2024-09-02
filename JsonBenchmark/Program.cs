using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace JsonBenchmark;

internal static class Program
{
    private static void Main()
    {
        BenchmarkRunner.Run<JsonPerformance>(new BenchmarkConfig());
    }
}

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddJob(Job.ShortRun
            .WithRuntime(CoreRuntime.Core90)
            .WithJit(Jit.Default)
            .WithPlatform(Platform.X64)
        );
        
        AddLogger(ConsoleLogger.Default);
        AddColumnProvider(BenchmarkDotNet.Columns.DefaultColumnProviders.Instance);
        AddExporter(BenchmarkDotNet.Exporters.HtmlExporter.Default);
    }
}