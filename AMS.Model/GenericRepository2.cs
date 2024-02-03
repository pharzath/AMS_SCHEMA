using AMS.Domain;
using AMS.Model.Models;
using Neo4jClient;
using Neo4jClient.Cypher;
using Neo4jClient.DataAnnotations;
using Newtonsoft.Json.Linq;
using Olive;
using QOQNOS.Core;

namespace AMS.Model;

public class GenericRepository : NeoRepositoryBase
{
    public GenericRepository(IGraphClient graphClient)
        : base(graphClient)
    {
    }

    public async Task CreateNewEntity<T>(T entity, string[]? labels = null)
        where T : IHaveId<Guid>
    {
        await _client.ConnectAsync();

        await _client.Cypher
            .Create(path =>
                path.Pattern<T>("a").Label(labels)
                    .Prop(() => entity))
            .ExecuteWithoutResultsAsync();
    }
    public async Task CreateNewEntitySafe<T>(T entity, string[]? labels = null)
        where T : IHaveId<Guid>
    {
        await _client.ConnectAsync();

        await _client.Cypher
            .Merge(path =>
                path.Pattern<T>("a")
                    .Label(labels)
                    .Constrain(a => a.Id == entity.Id))
            .OnCreate()
            .Set("a", () => entity)
            .ExecuteWithoutResultsAsync();
    }


    public ICypherFluentQuery Get<T>(Guid id) where T : EntityBase, IHaveId<Guid>
    {
        return _client.Cypher.Match(p => p.Pattern<T>("a").Constrain(a => a.Id == id));
    }

    public async Task Connect()
    {
        await _client.ConnectAsync();
    }

    public ICypherFluentQuery GetAll(string nodeType)
    {

        return _client.Cypher
            .Match($"(a:{nodeType})")
            .With("a")
            .OrderBy("a.nodeOrder");
    }

    public ICypherFluentQuery GetAll(string? parentType, EntityBase? parentEntity, string? rel,
        RelationshipDirection dir, string nodeType)
    {
        if (parentType is null)
            return GetAll(nodeType);


        if (parentEntity == null) return GetAll(nodeType);

        var query = _client.Cypher
            .Match(
                $"(a:{nodeType}){(dir == RelationshipDirection.Outgoing ? "<-" : "-")}[:{rel}]{(dir == RelationshipDirection.Incoming ? "->" : "-")}(b:{parentType})")
            .Where("b.id = $id")
            .WithParam("id", parentEntity.Id)
            .With("a")
            .OrderBy($"a.nodeOrder{parentType}")
            ;
        // Console.WriteLine(query.Query.DebugQueryText);
        // Console.WriteLine("       ---------------");
        return query;
    }

    public async Task DeleteEntityWithIdAndAllRelations(Guid entityId)
    {
        await _client.ConnectAsync();
        var cypherFluentQuery = _client.Cypher
            .Match("(a { id : $entityId})")
            .WithParams(new { entityId })
            .DetachDelete("a")
            ;
        var queryText = cypherFluentQuery.Query.DebugQueryText;
        Console.WriteLine(queryText);

        await cypherFluentQuery
            .ExecuteWithoutResultsAsync();
    }

    public async Task AddLabelToEntity(Guid nodeId, string? label)
    {
        await _client.ConnectAsync();
        var cypherFluentQuery = _client.Cypher
            .Match("(a)")
            .Where("a.id = $entityId")
            .WithParam("entityId", nodeId)
            .Set($"a:{label}")
            ;
        var queryText = cypherFluentQuery.Query.DebugQueryText;
        Console.WriteLine(queryText);

        await cypherFluentQuery
            .ExecuteWithoutResultsAsync();
    }

    public async Task Join(Guid aId, Guid bId, string relation, RelationshipDirection dir)
    {
        await _client.ConnectAsync();
        var cypherFluentQuery = _client.Cypher
                .Match("(a {id : $aId}) , (b {id : $bId})")
                .WithParams(new { aId, bId })
                .Create($"(a){"<".OnlyWhen(dir == RelationshipDirection.Outgoing)}-[:{relation}]-{">".OnlyWhen(dir == RelationshipDirection.Incoming)}(b)")
            ;
        var queryText = cypherFluentQuery.Query.DebugQueryText;
        Console.WriteLine(queryText);

        await cypherFluentQuery.ExecuteWithoutResultsAsync();
    }

    public async Task<EntityBase?> CreateNewChildEntity(Guid nodeId, string label, string? relation, RelationshipDirection dir, EntityBase entity)
    {
        await _client.ConnectAsync();
        var cypherFluentQuery = _client.Cypher
                .Match("(a)")
                .Where("a.id = $entityId")
                .WithParam("entityId", nodeId)
                .Create($"(a){"<".OnlyWhen(dir == RelationshipDirection.Outgoing)}-[:{relation}]-{">".OnlyWhen(dir == RelationshipDirection.Incoming)}(b:{label})")
                .Set("b = $entity ")
                .WithParam("entity", entity)
                .Return((b) => b.As<EntityBase>())
                .Limit(1)
            ;
        var queryText = cypherFluentQuery.Query.DebugQueryText;
        Console.WriteLine(queryText);

        var entityx = await cypherFluentQuery.ResultsAsync;
        return entityx.FirstOrDefault();
    }

    public async Task<IEnumerable<EntityBase>> GetAllEntityByLabelsTitle(IEnumerable<string> labelNames, string title)
    {
        await _client.ConnectAsync();
        var labelWhere = string.Join(" or ", labelNames.Select(x => $"a:{x}")).WithWrappers("(", ")");
        ICypherFluentQuery<EntityBase>? cypherFluentQuery;
        if (title.IsEmpty())
        {
            cypherFluentQuery = _client.Cypher
                .Match("(a)")
                .Where(labelWhere)
                .Return((a) => a.As<EntityBase>());

        }
        else
            cypherFluentQuery = _client.Cypher
                .Match("(a)")
                .Where(labelWhere)
                .AndWhere("toLower(a.title) CONTAINS toLower($title)")
                .WithParam("title", title)
                .Return((a) => a.As<EntityBase>());

        var queryText = cypherFluentQuery.Query.DebugQueryText;
        Console.WriteLine(queryText);

        var entity = await cypherFluentQuery?.ResultsAsync;
        return entity;

    }

    public async Task CreateNewRelation(Guid nodeId, Guid existingEntityId, string[] assignedLabels, string? relation,
        RelationshipDirection dir)
    {
        await _client.ConnectAsync();
        var cypherFluentQuery = _client.Cypher
            .Match("(a {id : $aId})", "(b {id : $bId})")
            .WithParams(new Dictionary<string, object>
            {
                ["aId"] = nodeId,
                ["bId"] = existingEntityId
            })
            .Set($"b:" + string.Join(":", assignedLabels))
            .Create($"(a){"<".OnlyWhen(dir == RelationshipDirection.Outgoing)}-[:{relation}]-{">".OnlyWhen(dir == RelationshipDirection.Incoming)}(b)");
        var queryText = cypherFluentQuery.Query.DebugQueryText;
        Console.WriteLine(queryText);

        await cypherFluentQuery.ExecuteWithoutResultsAsync();
    }

    public async Task<List<NodeRelation2>?> GetOutgoingRelatedNodes(string label, Guid entityId)
    {
        await _client.ConnectAsync();
        var cypherFluentQuery = _client.Cypher
            .Match("(a {id : $entityId})-[r]->(b)")
            .WithParam("entityId", entityId)
            .With($"b, r, b.nodeOrder{label} as nodeOrder")
            .OrderBy($"nodeOrder")
            .Return((r, b, nodeOrder) =>
                new NodeRelation2
                {
                    Json = b.As<string>(),
                    LabelNames = b.Labels(),
                    Relation = r.Type(),
                    NodeOrder = nodeOrder.As<int>()
                });
        Console.WriteLine(cypherFluentQuery.Query.DebugQueryText);

        var results = (await cypherFluentQuery.ResultsAsync).ToList();
        results.ForEach(x => x.Dir = RelationshipDirection.Outgoing);

        return results ?? null;
    }
    public async Task<List<NodeRelation2>?> GetIncomingRelatedNodes(string label, Guid entityId)
    {
        await _client.ConnectAsync();
        var cypherFluentQuery = _client.Cypher
            .Match("(a {id : $entityId})<-[r]-(b)")
            .WithParam("entityId", entityId)
            .With($"a, b, r, b.nodeOrder{label} as nodeOrder")
            .OrderBy($"nodeOrder")
            .Return((r, b, nodeOrder) =>
                new NodeRelation2
                {
                    Json = b.As<string>(),
                    LabelNames = b.Labels(),
                    Relation = r.Type(),
                    NodeOrder = nodeOrder.As<int>()
                });
        Console.WriteLine(cypherFluentQuery.Query.DebugQueryText);

        var results = (await cypherFluentQuery.ResultsAsync).ToList();
        results.ForEach(x => x.Dir = RelationshipDirection.Incoming);
        return results ?? null;
    }

    public class NodeRelation2
    {
        int? _nodeOrder;
        JObject? _jObj;
        EntityBase? _entity;
        public string Title => Entity.Title;
        public IEnumerable<AmsNeo4JNodeLabel> Labels { get; set; }
        public string Relation { get; set; }
        public RelationshipDirection Dir { get; set; }

        public EntityBase Entity
        {
            get => _entity ??= JObj.ToObject<EntityBase>();
            set => _entity = value;
        }

        public int? NodeOrder
        {
            get => _nodeOrder ?? 9999;
            set => _nodeOrder = value;
        }

        public JObject? JObj
        {
            get => _jObj ??= JObject.Parse(Json);
            init => _jObj = value;
        }

        public IEnumerable<string> LabelNames { get; set; }
        public string Json { get; set; }
    }

    public async Task UpdateNodeOrder(Guid entityId, string label, int i)
    {

        var cypherFluentQuery = _client.Cypher
            .Match("(a)")
            .Where("a.id = $entityId")
            .WithParam("entityId", entityId)
            .Set($"a.nodeOrder{label} = $nodeOrder")
            .WithParam("nodeOrder", i);

        Console.WriteLine(cypherFluentQuery.Query.DebugQueryText);

        await cypherFluentQuery
            .ExecuteWithoutResultsAsync();
    }

    public async Task SwapNodeOrder(Guid entityId1, Guid entityId2, string parentLabel)
    {
        var cypherFluentQuery = _client.Cypher
            .Match("(a {id: $entityId1 })", "(b {id: $entityId2 })")
            .WithParams(new Dictionary<string, object>
            {
                ["entityId1"] = entityId1,
                ["entityId2"] = entityId2,
            })
            .With($"a,b,a.nodeOrder{parentLabel} as aOrder,b.nodeOrder{parentLabel} as bOrder")
            .Set($"a.nodeOrder{parentLabel} = bOrder, b.nodeOrder{parentLabel} = aOrder")
            ;

        Console.WriteLine(cypherFluentQuery.Query.DebugQueryText);

        await cypherFluentQuery
            .ExecuteWithoutResultsAsync();

    }

    public async Task UpdateNodeProperties(EntityBase ent)
    {
        await _client.ConnectAsync();
        var cypherFluentQuery = _client.Cypher
            .Match("(entity)")
            .Where((EntityBase entity) => entity.Id == ent.Id)
            .Set("entity += $ent")
            .WithParam("ent", ent)

            ;
        var queryDebugQueryText = cypherFluentQuery.Query.DebugQueryText;
        Console.WriteLine(queryDebugQueryText);
        await cypherFluentQuery.ExecuteWithoutResultsAsync();
    }

    public async Task Save(JObject jObject, string? labelChain = null)
    {
        await _client.ConnectAsync();
        var cypherFluentQuery = _client.Cypher
                .Merge($"(entity{labelChain} {{ id : $id }})")
                .WithParam("id", jObject["id"])
                .OnCreate()
                .Set("entity = $ent")
                .OnMatch()
                .Set("entity = $ent, entity.modifiedAt = $modifiedAt, entity.modifyAction = $modifyAction")
                .WithParams(new { ent = jObject, modifiedAt = DateTime.UtcNow, modifyAction = "Schema Explorer" })
            ;

        var queryDebugQueryText = cypherFluentQuery.Query.DebugQueryText;
        Console.WriteLine(queryDebugQueryText);
        await cypherFluentQuery.ExecuteWithoutResultsAsync();

    }
}