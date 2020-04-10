using System;
using System.IO;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;

namespace benchmark.graphqldotnet
{
    public class GraphQLDotnetRunner
    {
        private ISchema _schema;
        private readonly DocumentExecuter _executor;
        private readonly string _sdl;

        public GraphQLDotnetRunner()
        {
            _executor = new DocumentExecuter();
            _sdl = File.ReadAllText("graphqldotnet/schema.graphql");
        }

        public void CreateSchema()
        {
            _schema = Schema.For(_sdl, _ =>
            {
                _.Types.Include<DroidType>();
                _.Types.Include<Query>();
            });
        }

        public Task<ExecutionResult> Execute(
            string query)
        {
            return _executor.ExecuteAsync(options =>
            {
                options.Schema = _schema;
                options.Query = query;
            });
        }
    }

    public class Droid
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Character
    {
        public string Name { get; set; }
    }

    public class Query
    {
        [GraphQLMetadata("hero")]
        public Droid GetHero()
        {
            return new Droid { Id = "1", Name = "R2-D2" };
        }
    }

    [GraphQLMetadata("Droid", IsTypeOf=typeof(Droid))]
    public class DroidType
    {
        public string Id(Droid droid) => droid.Id;
        public string Name(Droid droid) => droid.Name;

        public Character Friend(ResolveFieldContext context, Droid source)
        {
            return new Character { Name = "C3-PO" };
        }
    }
}
