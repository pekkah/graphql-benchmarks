using System;
using System.Linq;
using System.Threading.Tasks;
using benchmark.graphqldotnet;
using benchmark.tankagraphql;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace benchmark
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public class QueryBenchmarks
    {
        public static readonly string Query = @"{ hero { id name friend { name } } }";

        private GraphQLDotnetRunner _gqldnCreated;
        private TankaGraphQLRunner _tgqlCreated;

        [GlobalSetup]
        public void Setup()
        {
            _gqldnCreated = new GraphQLDotnetRunner();
            _gqldnCreated.CreateSchema();

            _tgqlCreated = new TankaGraphQLRunner();
            _tgqlCreated.CreateSchema();
        }

        [Benchmark]
        public async Task graphql_dotnet_query()
        {
            var result = await _gqldnCreated.Execute(Query);

            if (result.Errors != null && result.Errors.Count > 0)
                throw new InvalidOperationException($"Query execution error: {string.Join(",", result.Errors.Select(e => e.Message))}");
        }

        [Benchmark]
        public async Task tanka_graphql_query()
        {
            var result = await _tgqlCreated.Execute(Query);

            if (result.Errors != null && result.Errors.Count > 0)
                throw new InvalidOperationException($"Query execution error: {string.Join(",", result.Errors.Select(e => e.Message))}");
        }
    }
}