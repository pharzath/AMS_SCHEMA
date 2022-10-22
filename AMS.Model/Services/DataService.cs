using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AMS.Model.Models;
using Microsoft.EntityFrameworkCore;
using Olive;

namespace AMS.Model.Services
{
    public class DataService
    {
        public MyDbContext DbContext { get; }

        public DataService(MyDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public List<AmsNeo4JNodeLabel> GetClassLabels(AmsNeo4JNode node)
        {
            var lables = DbContext.AmsNeo4JNodeLabels.Where(x => x.NodeId == node.Id);
            return lables.ToList();
        }

        public List<AmsNeo4JNodeLabel> GetRootLabels(AmsNeo4JNode node)
        {
            var lables = DbContext.AmsNeo4JNodeLabels
                .Include(x => x.Properties)
                .Include(x => x.Constraints)
                .Include(x => x.Indices)

                .Include(x => x.ParentLabel)
                .ThenInclude(x => x.Properties)

                .Include(x => x.ChildLabels)

                .Where(x => x.NodeId == node.Id && x.ParentLabelId == null);
            return lables.ToList();
        }

        public List<AmsNeo4JNodeLabel> GetSubLabels(AmsNeo4JNode node)
        {
            var lables = DbContext.AmsNeo4JNodeLabels
                .Include(x => x.Properties)
                .Include(x => x.Constraints)
                .Include(x => x.Indices)

                .Include(x => x.ParentLabel)
                .ThenInclude(x => x.Properties)

                .Include(x => x.ChildLabels)

                .Where(x => x.NodeId == node.Id && x.ParentLabelId != null);
            return lables.ToList();
        }

        public Task<List<AmsNeo4JNodeRelation>> GetLabelRelationsAsync(AmsNeo4JNodeLabel lable)
        {
            return GetLabelRelationsAsync(new[] { lable });
        }
        public List<AmsNeo4JNodeRelation> GetLabelRelations(AmsNeo4JNodeLabel lable)
        {
            return GetLabelRelations(new[] { lable });
        }

        public Task<List<AmsNeo4JNodeRelation>> GetLabelRelationsAsync(IEnumerable<AmsNeo4JNodeLabel> lables)
        {
            var relations = AmsNeo4JNodeRelations(lables);
            var amsNeo4JNodeRelations = relations.ToListAsync();
            return amsNeo4JNodeRelations;
        }
        public List<AmsNeo4JNodeRelation> GetLabelRelations(IEnumerable<AmsNeo4JNodeLabel> lables)
        {
            var relations = AmsNeo4JNodeRelations(lables);
            var amsNeo4JNodeRelations = relations.ToList();
            return amsNeo4JNodeRelations;
        }

        IQueryable<AmsNeo4JNodeRelation> AmsNeo4JNodeRelations(IEnumerable<AmsNeo4JNodeLabel> lables)
        {
            var ids = lables.Select(x => x.Id);
            var relations = DbContext.AmsNeo4JNodeRelations
                .Include(x => x.From.Node)
                .ThenInclude(x => x.Label)
                .ThenInclude(x => x.ParentLabel)
                .Include(x => x.RelType)
                .ThenInclude(x => x.Properties)
                .Include(x => x.To.Node)
                .ThenInclude(x => x.Label)
                .ThenInclude(x => x.ParentLabel)

                .Include(x=>x.RelType)
                .ThenInclude(x=>x.Properties)
                .Include(x => x.RelType)
                .ThenInclude(x=>x.Constraints)
                .Include(x => x.RelType)
                .ThenInclude(x=>x.Indices)

                .Where(x => ids.Contains(x.FromFk.Value) || ids.Contains(x.ToFk.Value));
            return relations;
        }

        public Task<List<AmsNeo4JNodeConstraint>> GetNodeConstraintsAsync(AmsNeo4JNodeLabel label)
        {
            var relations = AmsNeo4JNodeConstraints(label);

            var amsNeo4JNodeRelations = relations.ToListAsync();
            return amsNeo4JNodeRelations;
        }
        public List<AmsNeo4JNodeConstraint> GetNodeConstraints(AmsNeo4JNodeLabel label)
        {
            var relations = AmsNeo4JNodeConstraints(label);

            var amsNeo4JNodeRelations = relations.ToList();
            return amsNeo4JNodeRelations;
        }

        IQueryable<AmsNeo4JNodeConstraint> AmsNeo4JNodeConstraints(AmsNeo4JNodeLabel label)
        {
            var relations = DbContext.AmsNeo4JNodeConstraints
                .Include(x => x.Label)
                .Where(x => x.LabelFk.Value == label.Id);
            return relations;
        }

        public Task<List<AmsNeo4JNodeRelation>> GetLabelRelationFromsAsync(AmsNeo4JNodeLabel lable)
        {
            var relations = GetRelationFroms(lable);

            return relations.ToListAsync();
        }

        public List<AmsNeo4JNodeRelation> GetLabelRelationFroms(AmsNeo4JNodeLabel lable)
        {
            var relations = GetRelationFroms(lable);

            return relations.ToList();
        }

        IQueryable<AmsNeo4JNodeRelation> GetRelationFroms(AmsNeo4JNodeLabel lable)
        {
            var relations = DbContext.AmsNeo4JNodeRelations
                .Include(x => x.From.Node)
                .ThenInclude(x => x.Label.ParentLabel)
                .ThenInclude(x => x.ParentLabel)
                .Include(x => x.From.Node)
                .ThenInclude(x => x.Label)
                .ThenInclude(x => x.ChildLabels)
                .Include(x => x.RelType)
                .ThenInclude(x => x.Properties)
                .Include(x => x.To.Node)
                .ThenInclude(x => x.Label)
                .ThenInclude(x => x.ParentLabel)
                .Include(x => x.To.Node)
                .ThenInclude(x => x.Label)
                .ThenInclude(x => x.ChildLabels)
                .Where(x => x.FromFk == lable.Id);
            return relations;
        }

        public Task<List<AmsNeo4JNodeRelation>> GetLabelRelationTosAsync(AmsNeo4JNodeLabel lable)
        {
            var relations = RelationsTos(lable);

            return relations.ToListAsync();
        }
        public List<AmsNeo4JNodeRelation> GetLabelRelationTos(AmsNeo4JNodeLabel lable)
        {
            var relations = RelationsTos(lable);

            return relations.ToList();
        }

        IQueryable<AmsNeo4JNodeRelation> RelationsTos(AmsNeo4JNodeLabel lable)
        {
            var relations = DbContext.AmsNeo4JNodeRelations
                .Include(x => x.From.Node)
                .ThenInclude(x => x.Label)
                //.ThenInclude(x => x.ParentLabel)
                .Include(x => x.RelType)
                .ThenInclude(x => x.Properties)
                .Include(x => x.To.Node)
                .ThenInclude(x => x.Label)
                //.ThenInclude(x => x.ParentLabel)
                .Where(x => x.ToFk.Value == lable.Id);
            return relations;
        }

        public void SaveLabel(AmsNeo4JNodeLabel label)
        {
            DbContext.AmsNeo4JNodeLabels.Update(label);
            DbContext.SaveChanges();
        }

        public List<AmsNeo4JNodeRelationType> GetRelationTypes()
        {
            var relationTypes = DbContext.AmsNeo4JNodeRelationTypes.ToList();
            return relationTypes;
        }

        public void SaveRelation(AmsNeo4JNodeRelation nodeRelation)
        {
            DbContext.AmsNeo4JNodeRelations.Update(nodeRelation);
            DbContext.SaveChanges();
        }

        public List<AmsNeo4JNode> GetNodes(string? search = null,
            IEnumerable<AmsmoduleDepartment>? amsmoduleDepartments = null,
            bool? andSelectDepartment = null)
        {
            var nodes = DbContext.AmsNeo4JNodes
                    .Include(x => x.Departments)
                    .ThenInclude(x => x.Department)

                    .Include(x => x.Label)
                    .ThenInclude(x => x.ParentLabel)

                    .Include(x => x.Label)
                    .ThenInclude(x => x.Properties)

                    .Include(x => x.Label)
                    .ThenInclude(x => x.Constraints)

                    .Include(x => x.Label)
                    .ThenInclude(x => x.Indices)

                    .AsQueryable()
                ;

            if (!string.IsNullOrEmpty(search))
            {
                var nodeIds = DbContext.AmsNeo4JNodeLabels
                    .Where(x => x.Name.Contains(search) || x.DisplayName.Contains(search))
                    .Select(x => x.Node.Id).ToList();

                nodes = nodes.Where(x =>
                                         nodeIds.Contains(x.Id) ||
                                         x.Name.Contains(search) ||
                                         x.DisplayName.Contains(search)
                //x.Label.Name.Contains(search) ||
                //x.Label.DisplayName.Contains(search)
                );


            }

            var schemaPageTypes = nodes.ToList();

            if (amsmoduleDepartments is { } && amsmoduleDepartments.Any())
            {
                var ids = amsmoduleDepartments.Select(d => d.DepartmentId).ToArray();
                schemaPageTypes = andSelectDepartment is true ?
                    schemaPageTypes.Where(x => x.Departments.Select(a => a.Department.DepartmentId).ContainsAll(ids)).ToList() :
                    schemaPageTypes.Where(x => x.Departments.Select(a => a.Department.DepartmentId).ContainsAny(ids)).ToList();
            }

            return schemaPageTypes;
        }

        public List<AmsNeo4JNodeRelationType> GetRelationTypes(
            string? search = null)
        {
            var relationTypes = DbContext.AmsNeo4JNodeRelationTypes
                .Include(x => x.Properties)
                .Include(x => x.Constraints)
                .Include(x => x.Indices)
                .AsQueryable()
                ;

            if (!string.IsNullOrEmpty(search))
            {
                relationTypes = relationTypes.Where(x => x.Name.Contains(search));
            }

            var list = relationTypes.ToList();

            return list;
        }


        public void SaveConstraint(AmsNeo4JNodeConstraint constraint)
        {
            DbContext.AmsNeo4JNodeConstraints.Update(constraint);
            DbContext.SaveChanges();
        }

        public void DeleteLabel(AmsNeo4JNodeLabel lbl)
        {
            var relations = DbContext.AmsNeo4JNodeRelations.Where(x => x.FromFk == lbl.Id || x.ToFk == lbl.Id);
            var constraints = DbContext.AmsNeo4JNodeConstraints.Where(x => x.Label.Id == lbl.Id);

            foreach (var relation in relations) DbContext.Remove(relation);
            foreach (var constraint in constraints) DbContext.Remove(constraint);
            DbContext.AmsNeo4JNodeLabels.Remove(lbl);

            DbContext.SaveChanges();
        }

        public void DeleteRelation(AmsNeo4JNodeRelation relation)
        {
            DbContext.AmsNeo4JNodeRelations.Remove(relation);
            DbContext.SaveChanges();
        }

        public List<AmsmoduleNodeDepartment> GetNodeDepartment(AmsNeo4JNode node)
        {
            var list = DbContext.AmsmoduleNodeDepartments
                .Include(x => x.Department)
                .Where(x => x.NodeId == node.Id).ToList();
            return list;
        }

        public List<AmsmoduleDepartment> GetDepartments()
        {
            return DbContext.AmsmoduleDepartments.ToList();
        }

        public void AddDepartmentToNode(AmsNeo4JNode node, AmsmoduleDepartment amsmoduleDepartment)
        {
            DbContext.AmsmoduleNodeDepartments.Update(new AmsmoduleNodeDepartment()
            {
                NodeId = node.Id,
                DepartmentId = amsmoduleDepartment.DepartmentId
            });
            DbContext.SaveChanges();
        }

        public void DeleteDepartmentFromNode(AmsmoduleNodeDepartment context)
        {
            DbContext.AmsmoduleNodeDepartments.Remove(context);
            DbContext.SaveChanges();
        }

        public void SaveLabelProperty(AmsNeo4JNodeLabelPropery prop)
        {
            DbContext.Update(prop);
            DbContext.SaveChanges();
        }

        public void DeleteConstraint(AmsNeo4JNodeConstraint constraint)
        {
            DbContext.AmsNeo4JNodeConstraints.Remove(constraint);
            DbContext.SaveChanges();
        }

        public void DeleteLabelProperty(AmsNeo4JNodeLabelPropery prop)
        {
            DbContext.AmsNeo4JNodeLabelProperies.Remove(prop);
            DbContext.SaveChanges();
        }

        public List<AmsNeo4JNodeIndex> GetNodeIndices(List<AmsNeo4JNodeLabel> labels)
        {
            var ids = labels.Select(x => x.Id);
            var relations = DbContext.AmsNeo4JNodeIndices
                .Include(x => x.Label)
                .Where(x => ids.Contains<int>(x.LabelId.Value));

            var amsNeo4JNodeRelations = relations.ToList();
            return amsNeo4JNodeRelations;

        }

        public void SaveIndex(AmsNeo4JNodeIndex index)
        {
            DbContext.AmsNeo4JNodeIndices.Update(index);
            DbContext.SaveChanges();
        }

        public void DeleteIndex(AmsNeo4JNodeIndex index)
        {
            DbContext.AmsNeo4JNodeIndices.Remove(index);
            DbContext.SaveChanges();
        }

        public void SaveNode(AmsNeo4JNode node)
        {
            DbContext.AmsNeo4JNodes.Update(node);
            DbContext.SaveChanges();
        }

        public void DeleteNode(AmsNeo4JNode node)
        {
            DbContext.AmsNeo4JNodes.Remove(node);
            DbContext.SaveChanges();
        }

        public List<AmsNeo4JNodeRelation> GetRelations(string? relationName)
        {
            var relations = DbContext
                .AmsNeo4JNodeRelations
                .Include(x => x.From.Node)
                .ThenInclude(x => x.Label)
                //.ThenInclude(x => x.ParentLabel)
                .Include(x => x.RelType)
                .ThenInclude(x => x.Properties)
                .Include(x => x.To.Node)
                .ThenInclude(x => x.Label)
                //.ThenInclude(x => x.ParentLabel)
                .Where(x => x.Name == relationName)
                ;

            return relations.ToList();
        }

        public void SaveRelationType(AmsNeo4JNodeRelationType relationType)
        {
            DbContext.AmsNeo4JNodeRelationTypes.Update(relationType);
            DbContext.SaveChanges();
        }

        public void SaveRelationTypeProperty(AmsNeo4JNodeRelationPropery property)
        {
            DbContext.AmsNeo4JNodeRelationProperies.Update(property);
            DbContext.SaveChanges();
        }

        public void DeleteRelationTypeProperty(AmsNeo4JNodeRelationPropery property)
        {
            DbContext.AmsNeo4JNodeRelationProperies.Remove(property);
            DbContext.SaveChanges();
        }

        public List<AmsNeo4JNodeConstraint> GetRelationTypeConstraints(AmsNeo4JNodeRelationType relationType)
        {
            var amsNeo4JNodeConstraints = DbContext.AmsNeo4JNodeConstraints.Where(x => x.RelTypeFk == relationType.Id);
            return amsNeo4JNodeConstraints.ToList();
        }

        public List<AmsNeo4JNodeLabel> GetLabelsTree(AmsNeo4JNode node)
        {
            var amsNeo4JNodeLabels = DbContext.AmsNeo4JNodeLabels
                .Include(x => x.ParentLabel)
                .Include(x => x.ChildLabels)
                .Where(x => x.NodeId == node.Id);

            return amsNeo4JNodeLabels.ToList();
        }

        public HashSet<AmsNeo4JNodeLabelTreenNode> GetChildLabels(AmsNeo4JNodeLabelTreenNode label)
        {
            var amsNeo4JNodeLabels = DbContext.AmsNeo4JNodeLabels
                .Where(x => x.ParentLabelId == label.Label.Id)
                .Select(x => new AmsNeo4JNodeLabelTreenNode()
                {
                    Label = x
                });

            return amsNeo4JNodeLabels.ToHashSet();
        }

        public HashSet<AmsNeo4JNodeLabelTreenNode> GetRootNodeLabels(AmsNeo4JNode node)
        {
            var amsNeo4JNodeLabels = DbContext.AmsNeo4JNodeLabels
                .Where(x => x.NodeId == node.Id && x.ParentLabelId == null)
                .Select(x => new AmsNeo4JNodeLabelTreenNode
                {
                    Label = x
                });
            return amsNeo4JNodeLabels.ToHashSet();
        }

        public AmsNeo4JNodeLabel? GetParentLabel(AmsNeo4JNodeLabel lbl)
        {
            return lbl.ParentLabelId == null
                ? null
                : DbContext.AmsNeo4JNodeLabels
                    .Include(x => x.Node)
                    .ThenInclude(x => x.Departments)

                    .Include(x => x.ParentLabel)

                    .Include(x => x.ChildLabels)

                    .Include(x => x.Properties)

                    .Include(x => x.Constraints)

                    .Include(x => x.Indices)

                    .FirstOrDefault(x => x.Id == lbl.ParentLabelId);
        }

        public List<AmsNeo4JNodeLabel> SearchLabels(string? search)
        {
            var labels = DbContext.AmsNeo4JNodeLabels
                .Include(x => x.Node)
                .ThenInclude(x => x.Departments)
                .Include(x => x.ParentLabel)
                .Include(x => x.ChildLabels)
                .Include(x => x.Properties)
                .Include(x => x.Constraints)
                .Include(x => x.Indices)
                .Where(x => x.Name.Contains(search) || x.DisplayName.Contains(search))
                ;

            return labels.ToList();
        }

        public List<AmsNeo4JNodeLabel> GetLabels(AmsNeo4JNode node)
        {
            var labels = DbContext.AmsNeo4JNodeLabels
                .Include(x => x.Node)
                .ThenInclude(x => x.Departments)
                .Include(x => x.ParentLabel)
                .Include(x => x.ChildLabels)
                .Include(x => x.Properties)
                .Include(x => x.Constraints)
                .Include(x => x.Indices)
                .Where(x => x.NodeId == node.Id);
            return labels.ToList();
        }

        public AmsNeo4JNodeLabel? GetLabel(int? lblId)
        {
            AmsNeo4JNodeLabel? label = DbContext.AmsNeo4JNodeLabels
                .Include(x => x.Node)
                .ThenInclude(x => x.Departments)
                .Include(x => x.ParentLabel)
                .Include(x => x.ChildLabels)
                .Include(x => x.Properties)
                .Include(x => x.Constraints)
                .Include(x => x.Indices)
                .FirstOrDefault(x => x.Id == lblId);

            return label;
        }

        public List<AmsNeo4JNodeLabelPropery> GetLabelProperties(int? labelId)
        {
            var properies = DbContext.AmsNeo4JNodeLabelProperies
                .Where(x => x.LabelId == labelId)
                .ToList();

            return properies;
        }

        public List<AmsNeo4JNodeLabelPropery> GetLabelProperties(AmsNeo4JNodeLabel label)
        {
            return GetLabelProperties(label.Id);
        }

        public IEnumerable<AmsNeo4JNodeLabelQuery> GetNodeQueries(AmsNeo4JNodeLabel label)
        {
            var queries = DbContext.AmsNeo4JNodeLabelQueries
                .Where(x => x.LabelId == label.Id)
                .AsEnumerable();
            return queries;
        }

        public IEnumerable<AmsNeo4JNodeLabel> GetLabelsByNameList(string[] names)
        {
            var labels = DbContext.AmsNeo4JNodeLabels
                .Where(x => names.Contains(x.Name))
                .AsEnumerable();
            return labels;
        }

        public void SaveLabelQuery(AmsNeo4JNodeLabelQuery query)
        {
            DbContext.AmsNeo4JNodeLabelQueries.Update(query);
            DbContext.SaveChanges();
        }

        public IEnumerable<AmsNeo4JNodeLabel> GetAllLabels()
        {
            var labels = DbContext.AmsNeo4JNodeLabels.AsEnumerable();
            return labels;
        }
    }
}
