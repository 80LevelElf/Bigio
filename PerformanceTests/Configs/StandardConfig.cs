using System.Collections.Generic;
using BenchmarkDotNet.Analyzers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;

namespace PerformanceTests.Configs
{
    public class StandardConfig : IConfig
    {
        public StandardConfig()
        {
            UnionRule = ConfigUnionRule.AlwaysUseLocal;
        }

        public IEnumerable<IColumn> GetColumns()
        {
            return new[] { PropertyColumn.Method , StatisticColumn.Median, StatisticColumn.OperationsPerSecond };
        }

        public IEnumerable<IExporter> GetExporters()
        {
            return new[] { new HtmlExporter() };
        }

        public IEnumerable<ILogger> GetLoggers()
        {
            return new ILogger[] { new ConsoleLogger() };
        }

        public IEnumerable<IDiagnoser> GetDiagnosers()
        {
            return new IDiagnoser[] { };
        }

        public IEnumerable<IAnalyser> GetAnalysers()
        {
            return new IAnalyser[] { EnvironmentAnalyser.Default };
        }

        public IEnumerable<IJob> GetJobs()
        {
            return new[]
                {
                    new Job
                    {
                        Mode = Mode.Throughput,
                        LaunchCount = 1,
                        IterationTime = 100
                    }
                };
        }

        public ConfigUnionRule UnionRule { get; private set; }
    }
}
