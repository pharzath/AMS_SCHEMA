using AMS.Model;
using AMS.Model.Models;
using AMS.Model.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Neo4jClient;
using Neo4jClient.Cypher;
using Neo4jClient.DataAnnotations.Extensions.Driver;
using Newtonsoft.Json.Linq;

namespace AMS_SCHEMA.Pages.Schema.TestData.Components
{
    public class MyCustomTreeService
    {
        readonly GenericRepository _gr;
        readonly DataService _dataService;
        //public List<AmsNeo4JNodeLabel> CachedLabels { get; }
        //public List<AmsNeo4JNodeRelationType> CachedRelations { get; }

        public MyCustomTreeService(GenericRepository gr, DataService dataService)
        {
            _gr = gr;
            _dataService = dataService;
            //CachedLabels = _dataService.DbContext.AmsNeo4JNodeLabels.OrderBy(x => x.ParentLabelId).ToList();
            //CachedRelations = _dataService.DbContext.AmsNeo4JNodeRelationTypes.ToList();
        }

        public async Task<List<MyNode>> GetChildNodes(MyNode myNode)
        {
            var outgoing = await _gr.GetOutgoingRelatedNodes(myNode.LabelsChainText, myNode.Entity.Id);
            if (outgoing != null)
                foreach (var relation in outgoing)
                {
                    relation.Labels = _dataService.GetLabels(relation.LabelNames);
                    var node = CreateNode(myNode, relation, RelationshipDirection.Outgoing);
                    if (node == null) continue;
                    myNode.Nodes.Add(node);
                }

            var incoming = await _gr.GetIncomingRelatedNodes(myNode.LabelsChainText, myNode.Entity.Id);
            if (incoming != null)
                foreach (var relation in incoming)
                {
                    relation.Labels = _dataService.GetLabels(relation.LabelNames);
                    var node = CreateNode(myNode, relation, RelationshipDirection.Incoming);
                    if (node == null) continue;
                    myNode.Nodes.Add(node);
                }

            myNode.Loading = false;
            var childNodes = myNode.Nodes;
            return childNodes;
        }

        public List<AmsNeo4JNodeLabel> GetLabelHirarchy(AmsNeo4JNodeLabel label)
        {
            return _dataService.GetParentLabelsAndThis(label);
        }

        public MyNode? CreateNode(MyNode? parentNode, GenericRepository.NodeRelation2 relation, RelationshipDirection direction)
        {
            var relationNodeOrder = relation.NodeOrder ?? 999;
            var opacity = 1d;
            if (parentNode != null && relation.Entity.Equals(parentNode.ParentNode?.Entity))
            {
                opacity = .1;
                relationNodeOrder = 9999;
            }

            var labels = relation.Labels;
            if (labels == null || !labels.Any())
                labels = _dataService.GetLabels().Where(x =>
                relation.Labels
                    .Select(x => x.Name)
                    .Contains(x.Name))
                ;

            var rel = _dataService.GetRelationTypes()
                .FirstOrDefault(x => x.Name == relation.Relation) ?? new AmsNeo4JNodeRelationType() { DisplayName = "" };

            return new MyNode(relation.Entity)
            {
                Jobj = relation.JObj,
                Labels = labels.ToList(),
                Relation = rel,
                RelType = relation.Relation,
                Direction = direction,
                ParentNode = parentNode,
                Order = relationNodeOrder,
                Style = new NodeStyle
                {
                    Opacity = opacity
                }
            };
        }

        public async Task MoveNodeUpDown(MyNode node, int upDown = -1)
        {

            var nodeParentNode = node.ParentNode;
            if (nodeParentNode == null) return;

            var nodes = nodeParentNode.Nodes;

            var nextNode = nodes.First(x => x.Order == node.Order + (1 * upDown));

            await _gr.SwapNodeOrder(node.Entity.Id, nextNode.Entity.Id, node.ParentNode.LabelsChainText);

            (node.Order, nextNode.Order) = (nextNode.Order, node.Order);
        }

        public async Task UpdateOrders(MyNode node)
        {
            var lst = node.Nodes.ToList();
            for (var i = 0; i < lst.Count; i++)
            {
                var n = lst[i];
                n.Order = i;
                await _gr.UpdateNodeOrder(n.Entity.Id, n.ParentNode.LabelsChainText, i);
            }
        }

        public async Task UpdateNodeOrderSaved(MyNode myNode)
        {
            await _gr.UpdateNodeOrder(myNode.Entity.Id, myNode.ParentNode!.LabelsChainText, myNode.Order);
        }


        public async Task<IEnumerable<JObject>> GetAllDataByLabel(AmsNeo4JNodeLabel label)
        {
            await _gr.Connect();
            var fluentQuery = _gr.GetAll(label.Name);
            var res = await fluentQuery
                .Return(a => a.As<string>())
                .ResultsAsync;

            var enumerable = res.Select(JObject.Parse);
            return enumerable;
        }

        /*
        public async Task<IEnumerable<JObject>> GetAllDataByLabel2(AmsNeo4JNodeLabel label)
        {
            await _gr.Connect();
            var fluentQuery = _gr.GetAll(label.Name);
            var res = await fluentQuery
                .Return(a => a.As<string>())
                .ResultsAsync;

            var enumerable = res.Select(JObject.Parse);
            return enumerable;
        }
        */

    }
}
