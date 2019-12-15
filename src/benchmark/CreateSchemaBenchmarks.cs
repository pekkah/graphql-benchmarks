using benchmark.graphqldotnet;
using benchmark.tankagraphql;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace benchmark
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public class CreateSchemaBenchmarks
    {
        private GraphQLDotnetRunner _gqldn;
        private TankaGraphQLRunner _tgql;

        [GlobalSetup]
        public void Setup()
        {
            _gqldn = new GraphQLDotnetRunner();
            _tgql = new TankaGraphQLRunner();
        }

        [Benchmark]
        public void graphql_dotnet_create_schema()
        {
            _gqldn.CreateSchema();
        }

        [Benchmark]
        public void tanka_graphql_create_schema()
        {
            _tgql.CreateSchema();
        }
    }
}