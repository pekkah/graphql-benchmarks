using System.IO;
using System.Threading.Tasks;
using Tanka.GraphQL;
using Tanka.GraphQL.SchemaBuilding;
using Tanka.GraphQL.SDL;
using Tanka.GraphQL.TypeSystem;
using Tanka.GraphQL.ValueResolution;

namespace benchmark.tankagraphql
{
    public class TankaGraphQLRunner
    {
        private readonly string _sdl;
        private ISchema _schema;

        public TankaGraphQLRunner()
        {
            _sdl = File.ReadAllText("schema.graphql");
        }

        public void CreateSchema()
        {
            _schema = new SchemaBuilder()
                .Sdl(_sdl)
                .UseResolversAndSubscribers(new SchemaResolvers())
                .Build();
        }

        public Task<ExecutionResult> Execute(
            string query)
        {
            return Executor.ExecuteAsync(new ExecutionOptions
            {
                Schema = _schema,
                Document = Parser.ParseDocument(query)
            });
        }
    }

    public class SchemaResolvers : ObjectTypeMap
    {
        public SchemaResolvers()
        {
            this["Query"] = new FieldResolversMap
            {
                {"hero", context => ResolveSync.As(new Droid {Id = "1", Name = "R2-D2"})}
            };

            this["Droid"] = new DroidResolvers();
            this["Character"] = new CharacterResolvers();
        }
    }

    public class CharacterResolvers : FieldResolversMap
    {
        public CharacterResolvers()
        {
            Add("name", Resolve.PropertyOf<Character>(c => c.Name));
        }
    }

    public class DroidResolvers : FieldResolversMap
    {
        public DroidResolvers()
        {
            Add("id", Resolve.PropertyOf<Droid>(d => d.Id));
            Add("name", Resolve.PropertyOf<Droid>(d => d.Name));
            Add("friend", context => ResolveSync.As(
                new Character
                {
                    Name = "C3-PO"
                }));
        }
    }

    public class Character
    {
        public string Name { get; set; }
    }

    public class Droid
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}