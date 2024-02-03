using Neo4jClient;
using Neo4jClient.DataAnnotations;
using QOQNOS.Core;

namespace AMS.Domain;

public class NeoRepositoryBase
{
    internal readonly IGraphClient _client;

    public NeoRepositoryBase(IGraphClient graphClient)
    {
        _client = graphClient.WithAnnotations();

    }

    public async Task<string> Join<A, R, B>(Guid aId, Guid bId)
        where A : IHaveId<Guid>
        where B : IHaveId<Guid>
    {
        await _client.ConnectAsync();
        var cypherFluentQuery = _client.Cypher
            .Match(pathA => pathA.Pattern<A>("a").Constrain(a => a.Id == aId) , 
                   pathB => pathB.Pattern<B>("b").Constrain(b => b.Id == bId)
            )
            .Merge(path => path.Pattern<A, R, B>("a", null, "b"))
            ;
        var queryQueryText = cypherFluentQuery.Query.QueryText;
        Console.WriteLine(queryQueryText);
         await cypherFluentQuery
              .ExecuteWithoutResultsAsync();

        return queryQueryText;
    }

}