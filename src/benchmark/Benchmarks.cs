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
    public class Benchmarks
    {
        public static readonly string Query = @"{ hero { id name friend { name } } }";

        private GraphQLDotnetRunner _gqldn;
        private GraphQLDotnetRunner _gqldnCreated;

        private TankaGraphQLRunner _tgql;
        private TankaGraphQLRunner _tgqlCreated;

        [GlobalSetup]
        public void Setup()
        {
            _gqldn = new GraphQLDotnetRunner();
            _gqldnCreated = new GraphQLDotnetRunner();
            _gqldnCreated.CreateSchema();

            _tgql = new TankaGraphQLRunner();
            _tgqlCreated = new TankaGraphQLRunner();
            _tgqlCreated.CreateSchema();
        }

        [Benchmark]
        public void graphql_dotnet_create_schema()
        {
            _gqldn.CreateSchema();
        }

        [Benchmark]
        public async Task graphql_dotnet_query()
        {
            var result = await _gqldnCreated.Execute(Query);

            if (result.Errors != null && result.Errors.Count > 0)
                throw new InvalidOperationException($"Query execution error: {string.Join(",", result.Errors.Select(e => e.Message))}");
        }

        [Benchmark]
        public void tanka_graphql_create_schema()
        {
            _tgql.CreateSchema();
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