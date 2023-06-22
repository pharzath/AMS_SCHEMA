using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AMS.Model.Models;
using AMS_SCHEMA.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Olive;
using QOQNOS.Core;

namespace AMS.Model.Services
{
    public class DataService : IDisposable
    {
        readonly MyCachProvider _cachProvider;
        public MyDbContext DbContext { get; }
        public AmsNeo4JProject? SelectedProject { get; set; }

        public DataService(MyDbContext dbContext,
                           MyCachProvider cachProvider)
        {
            _cachProvider = cachProvider;
            DbContext = dbContext;
        }


        object RootLabelLock = 0;
        public List<AmsNeo4JNodeLabel> GetRootLabels(AmsNeo4JNode node)
        {
            lock (RootLabelLock)
            {
                var lables = GetLabels()
                    .Where(x => x?.NodeId == node.Id && x?.ParentLabelId == null);
                var data = lables.ToList();
                return data;
            }
        }

        public List<AmsNeo4JNodeLabel> GetSubLabels(AmsNeo4JNode node)
        {
            var lables = GetLabels()
                .Where(x => x?.NodeId == node.Id && x?.ParentLabelId != null);
            var data = lables.ToList();
            return data;
        }

        public List<AmsNeo4JNodeRelation> GetLabelRelations(AmsNeo4JNodeLabel lable)
        {
            return GetLabelRelations(new[] { lable });
        }

        public List<AmsNeo4JNodeRelation> GetLabelRelations(IEnumerable<AmsNeo4JNodeLabel> lables)
        {
            var ids = lables.Select(x => x.Id);
            var relations = GetRelations()
                .Where(x => ids.Contains(x.FromFk.Value) || ids.Contains(x.ToFk.Value));

            var amsNeo4JNodeRelations = relations.ToList();
            return amsNeo4JNodeRelations;

        }

        IQueryable<AmsNeo4JNodeRelation> AmsNeo4JNodeRelations(IEnumerable<long> ids)
        {
            var relations = DbContext.AmsNeo4JNodeRelations

                .Include(x => x.From.Node)
                .ThenInclude(x => x.Label)
                .ThenInclude(x => x.ParentLabel)

                .Include(x => x.To.Node)
                .ThenInclude(x => x.Label)
                .ThenInclude(x => x.ParentLabel)

                .Include(x => x.RelType)
                .ThenInclude(x => x.Properties)

                .Include(x => x.RelType)
                .ThenInclude(x => x.Constraints)

                .Include(x => x.RelType)
                .ThenInclude(x => x.Indices)

                .Where(x => ids.Contains(x.FromFk.Value) || ids.Contains(x.ToFk.Value));
            return relations;
        }

        public List<AmsNeo4JNodeConstraint>? GetNodeConstraints()
        {
            return _cachProvider.GetOrCreate(
                _cachProvider.CreateCachKey<AmsNeo4JNodeConstraint>(nameof(GetNodeConstraints)), _ =>
            {
                return DbContext.AmsNeo4JNodeConstraints
                    .Include(x => x.Label)
                    .ToList();
            });
        }


        public List<AmsNeo4JNodeConstraint> GetNodeConstraints(AmsNeo4JNodeLabel label)
        {
            return GetNodeConstraints()?.Where(x => x.LabelFk.Value == label.Id).ToList();
        }

        IQueryable<AmsNeo4JNodeConstraint> AmsNeo4JNodeConstraints(AmsNeo4JNodeLabel label)
        {
            var relations = DbContext.AmsNeo4JNodeConstraints
                .Include(x => x.Label)
                .Where(x => x.LabelFk.Value == label.Id);
            return relations;
        }

        public List<AmsNeo4JNodeRelation> GetLabelRelationFroms(AmsNeo4JNodeLabel lable)
        {
            return GetRelations().Where(x => x.FromFk == lable.Id).ToList();
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

        public List<AmsNeo4JNodeRelation> GetLabelRelationTos(AmsNeo4JNodeLabel lable)
        {
            return GetRelations().Where(x => x.ToFk == lable.Id).ToList();

        }

        IQueryable<AmsNeo4JNodeRelation> RelationsTos(AmsNeo4JNodeLabel lable)
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
                .Where(x => x.ToFk.Value == lable.Id);
            return relations;
        }

        public async Task SaveLabel(AmsNeo4JNodeLabel label)
        {
            if (label.Node.Name.StartsWith("New"))
                label.Node.Name = label.Name;
            DbContext.AmsNeo4JNodeLabels.Update(label);
            await DbContext.SaveChangesAsync();
            _cachProvider.ExpireCachKeys(label);
        }

        public List<AmsNeo4JNodeRelationType>? GetRelationTypes()
        {
            return _cachProvider.GetOrCreate(_cachProvider.CreateCachKey<AmsNeo4JNodeRelationType>(nameof(GetRelationTypes)), _ =>
            {
                var relationTypes = DbContext.AmsNeo4JNodeRelationTypes
                    .Include(x => x.Properties)
                    .Include(x => x.Constraints)
                    .Include(x => x.Indices)
                    .ToList();
                return relationTypes;
            });
        }

        public void SaveRelation(AmsNeo4JNodeRelation nodeRelation)
        {
            DbContext.AmsNeo4JNodeRelations.Update(nodeRelation);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(nodeRelation);
        }

        public IQueryable<AmsNeo4JNode> GetNodes(string? search = null,
            IEnumerable<AmsNeo4JDepartment>? amsmoduleDepartments = null,
            bool? andSelectDepartment = null)
        {
            var nodes = _cachProvider.GetOrCreate(
                _cachProvider.CreateCachKey<AmsNeo4JNode>(nameof(GetNodes)),
                _ =>
                {
                    return DbContext.AmsNeo4JNodes

                            .Include(x => x.Departments)

                            .Include(x => x.Label)
                            .ThenInclude(x => x.ParentLabel)

                            .Include(x => x.Label)
                            .ThenInclude(x => x.Properties)

                            .Include(x => x.Label)
                            .ThenInclude(x => x.Constraints)

                            .Include(x => x.Label)
                            .ThenInclude(x => x.Microservice)
                            .ThenInclude(x => x.Modules)

                            .Include(x => x.Label)
                            //.ThenInclude(x => x.ModuleSettings.Where(z=>z.LabelFk == x.Id ))

                            .Include(x => x.Label)
                            .ThenInclude(x => x.Indices)

                            .Where(x => SelectedProject == null || x.ProjectFk == SelectedProject.Id)
                            //.ToList()
                        ;
                });



            if (!string.IsNullOrEmpty(search))
            {
                var nodeIds = GetLabels()
                    .Where(x => x.Name.ToLower().Contains(search.OrEmpty().ToLower()) || x.DisplayName.Contains(search))
                    .Select(x => x.Node.Id)
                    .ToList();

                nodes = nodes.Where(x =>
                    nodeIds.Contains(x.Id) ||
                    x.Name.Contains(search) ||
                    x.DisplayName.OrEmpty().Contains(search)
                );
                //.ToList();


            }

            //var schemaPageTypes = nodes.ToList();

            if (amsmoduleDepartments is null || !amsmoduleDepartments.Any())
                return nodes;


            var ids = amsmoduleDepartments.Select(d => d.DepartmentId).ToArray();
            nodes = andSelectDepartment is true
                ? nodes.Where(x => x.Departments.Select(a => a.Department.DepartmentId).ContainsAll(ids))//.ToList()
                : nodes.Where(x => x.Departments.Select(a => a.Department.DepartmentId).ContainsAny(ids));//.ToList();


            return nodes;
        }

        public List<AmsNeo4JNodeRelationType> GetRelationTypes(string? search = null)
        {
            var relationTypes = GetRelationTypes();


            if (!string.IsNullOrEmpty(search))
            {
                relationTypes = relationTypes.Where(x => x.Name.Contains(search)).ToList();
            }

            return relationTypes;
        }


        public void SaveConstraint(AmsNeo4JNodeConstraint constraint)
        {
            DbContext.AmsNeo4JNodeConstraints.Update(constraint);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(constraint);
        }

        public void DeleteLabel(AmsNeo4JNodeLabel lbl)
        {
            var relations = DbContext.AmsNeo4JNodeRelations.Where(x => x.FromFk == lbl.Id || x.ToFk == lbl.Id);
            var constraints = DbContext.AmsNeo4JNodeConstraints.Where(x => x.Label.Id == lbl.Id);

            using var transaction = DbContext.Database.BeginTransaction();
            try
            {
                foreach (var relation in relations)
                {
                    DeleteRelation(relation);
                }

                foreach (var constraint in constraints)
                {
                    DeleteConstraint(constraint);
                }

                DbContext.AmsNeo4JNodeLabels.Remove(lbl);

                DbContext.SaveChanges();

                _cachProvider.ExpireCachKeys(lbl);

                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
            }
        }

        public void DeleteRelation(AmsNeo4JNodeRelation relation)
        {
            DbContext.AmsNeo4JNodeRelations.Remove(relation);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(relation);
        }

        public List<AmsNeo4JNodeDepartment> GetNodeDepartment(AmsNeo4JNode node)
        {
            var list = _cachProvider.GetOrCreate(_cachProvider.CreateCachKey(nameof(GetNodeDepartment), node), _ =>
            {
                return DbContext.AmsNeo4JNodeDepartments
                    .Include(x => x.Department)
                    .Where(x => x.NodeId == node.Id)
                    .ToList();
            });

            return list;
        }

        public IQueryable<AmsNeo4JDepartment> GetDepartments()
        {
            var list = _cachProvider.GetOrCreate(
                _cachProvider.CreateCachKey<AmsNeo4JDepartment>(nameof(GetDepartments), SelectedProject?.Id ?? 0), _ =>
                {
                    return DbContext.AmsNeo4JDepartments.Where(x =>
                        SelectedProject == null || SelectedProject.Id == x.ProjectFk);
                }
                //.ToList()
            );
            return list;
        }

        public void AddDepartmentToNode(AmsNeo4JNode node, AmsNeo4JDepartment amsmoduleDepartment)
        {
            DbContext.AmsNeo4JNodeDepartments.Update(new AmsNeo4JNodeDepartment()
            {
                NodeId = node.Id,
                DepartmentId = amsmoduleDepartment.DepartmentId
            });
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(node);
            _cachProvider.ExpireCachKeys(amsmoduleDepartment);
        }

        public void DeleteDepartmentFromNode(AmsNeo4JNodeDepartment context)
        {
            DbContext.AmsNeo4JNodeDepartments.Remove(context);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(context);
        }

        public void SaveLabelProperty(AmsNeo4JNodeLabelProperty prop)
        {
            DbContext.Update(prop);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(prop);
        }

        public void DeleteConstraint(AmsNeo4JNodeConstraint constraint)
        {
            DbContext.AmsNeo4JNodeConstraints.Remove(constraint);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(constraint);
        }

        public void DeleteLabelProperty(AmsNeo4JNodeLabelProperty prop)
        {
            DbContext.AmsNeo4JNodeLabelProperties.Remove(prop);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(prop);
        }

        public List<AmsNeo4JNodeIndex> GetNodeIndices(List<AmsNeo4JNodeLabel> labels)
        {
            var ids = labels.Select(x => x.Id);
            var list = _cachProvider.GetOrCreate(_cachProvider.CreateCachKey<AmsNeo4JNodeLabel>(nameof(GetNodeIndices)), _ =>
            {
                return DbContext.AmsNeo4JNodeIndices
                    .Include(x => x.Label)
                    .Where(x => ids.Contains<int>(x.LabelId))
                    .ToList();
            });

            return list;

        }

        public void SaveIndex(AmsNeo4JNodeIndex index)
        {
            DbContext.AmsNeo4JNodeIndices.Update(index);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(index);

        }

        public void DeleteIndex(AmsNeo4JNodeIndex index)
        {
            DbContext.AmsNeo4JNodeIndices.Remove(index);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(index);

        }

        public void SaveNode(AmsNeo4JNode node)
        {
            node.ProjectFk = SelectedProject?.Id ?? 1;
            DbContext.AmsNeo4JNodes.Update(node);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(node);
        }

        /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
        /// <exception cref="DbUpdateConcurrencyException">A concurrency violation is encountered while saving to the database.
        ///                 A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///                 This is usually because the data in the database has been modified since it was loaded into memory.</exception>
        /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
        public async Task SaveNodeAsync(AmsNeo4JNode node)
        {
            node.ProjectFk = SelectedProject?.Id ?? 1;
            DbContext.AmsNeo4JNodes.Update(node);
            await DbContext.SaveChangesAsync();
            _cachProvider.ExpireCachKeys(node);
        }

        public void DeleteNode(AmsNeo4JNode node)
        {
            DbContext.AmsNeo4JNodes.Remove(node);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(node);
        }

        public List<AmsNeo4JNodeRelation> GetRelations()
        {
            var list = _cachProvider.GetOrCreate(_cachProvider.CreateCachKey<AmsNeo4JNodeRelation>(nameof(GetRelations), typeof(AmsNeo4JNodeLabel)), _ =>
            {
                return DbContext
                        .AmsNeo4JNodeRelations
                        .Include(x => x.From.Node)
                        .ThenInclude(x => x.Label.ParentLabel)

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

                        // .Where(x => SelectedProject == null || 
                        // x.From?.Node.ProjectFk == SelectedProject.Id || 
                        // x.To?.Node.ProjectFk == SelectedProject.Id)
                        .ToList()
                    ;
            });

            return list;

        }
        public List<AmsNeo4JNodeRelation> GetRelations(string? relationName)
        {
            var amsNeo4JNodeRelations = GetRelations()
                .Where(x => x.RelType.Name == relationName)
                .ToList();
            return amsNeo4JNodeRelations;
        }

        public void SaveRelationType(AmsNeo4JNodeRelationType relationType)
        {
            DbContext.AmsNeo4JNodeRelationTypes.Update(relationType);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(relationType);
        }

        public void SaveRelationTypeProperty(AmsNeo4JNodeRelationProperty property)
        {
            DbContext.AmsNeo4JNodeRelationProperties.Update(property);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(property);

        }

        public void DeleteRelationTypeProperty(AmsNeo4JNodeRelationProperty property)
        {
            DbContext.AmsNeo4JNodeRelationProperties.Remove(property);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(property);
        }

        public List<AmsNeo4JNodeConstraint> GetRelationTypeConstraints(AmsNeo4JNodeRelationType relationType)
        {
            var amsNeo4JNodeConstraints = GetNodeConstraints().Where(x => x.RelTypeFk == relationType.Id);
            return amsNeo4JNodeConstraints.ToList();
        }

        public List<AmsNeo4JNodeLabel> GetParentLabelsAndThis(AmsNeo4JNodeLabel label)
        {
            var lst = new List<AmsNeo4JNodeLabel>();

            var lbl = label;
            while (lbl != null)
            {
                lst.Add(lbl);

                lbl = GetLabels()
                    .FirstOrDefault(x => x.Id == lbl.ParentLabelId);
            }

            return lst;
        }
        public List<AmsNeo4JNodeLabel> GetParentLabels(AmsNeo4JNodeLabel label)
        {
            var lst = new List<AmsNeo4JNodeLabel>();

            var lbl = label.ParentLabel;
            while (lbl != null)
            {
                lst.Add(lbl);

                lbl = GetLabels()
                    .FirstOrDefault(x => x.NodeId == lbl.ParentLabelId);
            }

            return lst;
        }

        public HashSet<AmsNeo4JNodeLabelTreenNode> GetChildLabels(AmsNeo4JNodeLabelTreenNode label)
        {
            //var list = _cachProvider.GetOrCreate(_cachProvider.CreateCachKey(nameof(GetParentLabelsAndThis), label, typeof(AmsNeo4JNodeLabel)),

            var labels = GetLabels()
                .Where(x => x.ParentLabelId == label.Label.Id)
                .Select(x => new AmsNeo4JNodeLabelTreenNode()
                {
                    Label = x
                });

            return labels.ToHashSet();
        }

        public HashSet<AmsNeo4JNodeLabelTreenNode> GetRootNodeLabels(AmsNeo4JNode node)
        {
            var labels = GetLabels()
                .Where(x => x.NodeId == node.Id && x.ParentLabelId == null)
                .Select(x => new AmsNeo4JNodeLabelTreenNode
                {
                    Label = x
                });
            return labels.ToHashSet();
        }

        public List<AmsNeo4JNodeLabel?> GetLabels()
        {
            var list = _cachProvider.GetOrCreate(
                _cachProvider.CreateCachKey<AmsNeo4JNodeLabel>(nameof(GetLabels), SelectedProject?.Id ?? 0),
                _ =>
                {
                    return DbContext.AmsNeo4JNodeLabels

                        .Include(x => x.Node)
                        .ThenInclude(x => x.Departments)

                        .Include(x => x.ParentLabel)

                        .Include(x => x.ChildLabels)

                        .Include(x => x.Properties)

                        .Include(x => x.Constraints)

                        .Include(x => x.Indices)
                        
                        .Include(x => x.InheriteFromLabel)

                        .Include(x => x.Microservice)
                        .ThenInclude(x => x.Modules)
                        //.AsNoTrackingWithIdentityResolution()

                        .Where(x => SelectedProject == null || x.Node.ProjectFk == SelectedProject.Id)

                        .OrderBy(x => x.ParentLabelId)

                        .ToList()
                        ;

                });

            return list;

        }

        public AmsNeo4JNodeLabel? GetParentLabel(AmsNeo4JNodeLabel lbl)
        {
            return lbl.ParentLabelId == null
                ? null
                : GetLabels().FirstOrDefault(x => x.Id == lbl.ParentLabelId);
        }

        public List<AmsNeo4JNodeLabel?> SearchLabels(string? search)
        {
            return GetLabels()
                .Where(x => x != null && (x.Name.ToLower().Contains(search.ToLower()) || x.DisplayName.OrEmpty().Contains(search)))
                .ToList();
        }

        public List<AmsNeo4JNodeLabel> GetLabels(AmsNeo4JNode node)
        {
            var labels = GetLabels()
                .Where(x => x.NodeId == node.Id);
            return labels.ToList();
        }

        public AmsNeo4JNodeLabel? GetLabel(long? lblId)
        {
            var label = GetLabels()
                .FirstOrDefault(x => x.Id == lblId);

            return label;
        }

        public List<AmsNeo4JNodeLabelProperty> GetLabelProperties(long labelId)
        {
            var properies = _cachProvider.GetOrCreate(
                _cachProvider.CreateCachKey<AmsNeo4JNodeLabelProperty>(nameof(GetLabelProperties), labelId),
                _ =>
                {
                    return DbContext.AmsNeo4JNodeLabelProperties
                        .Where(x => x.LabelId == labelId)

                        .ToList();
                });


            return properies;
        }

        public List<AmsNeo4JNodeLabelProperty> GetLabelProperties(AmsNeo4JNodeLabel label)
        {
            return GetLabelProperties(label.Id);
        }

        public List<AmsNeo4JNodeLabelQuery> GetNodeQueries(AmsNeo4JNodeLabel label)
        {
            var queries = _cachProvider.GetOrCreate(
                _cachProvider.CreateCachKey(nameof(GetNodeQueries), label),
                _ =>
                {
                    return DbContext.AmsNeo4JNodeLabelQueries
                        .Where(x => x.LabelId == label.Id)
                        .ToList();
                });

            return queries;
        }

        public List<AmsNeo4JNodeLabel> GetLabelsByNameList(string[] names)
        {
            var labels = GetLabels()
                .Where(x => names.Contains(x.Name))
                .ToList();
            return labels;
        }

        public void SaveLabelQuery(AmsNeo4JNodeLabelQuery query)
        {
            DbContext.AmsNeo4JNodeLabelQueries.Update(query);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(query);
        }

        public List<AmsNeo4JNodeLabel> GetLabels(IEnumerable<string> labelNames)
        {
            return GetLabels()
                    .Where(x => labelNames.Contains(x.Name))
                    .OrderBy(x => x.ParentLabelId)
                    .ToList();
        }

        public void ClearCach()
        {
            _cachProvider.Clear();
        }

        public void DeleteLabelRequire(AmsNeo4JNodeLabelRequiredRelation context)
        {
            DbContext.AmsNeo4JNodeLabelRequiredRelations.Remove(context);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(context);
        }

        public List<AmsNeo4JNodeLabelRequiredRelation> GetLabelRequires()
        {
            return _cachProvider.GetOrCreate(_cachProvider.CreateCachKey<AmsNeo4JNodeLabelRequiredRelation>(nameof(GetLabelRequires)),
                _ =>
                {
                    var list = DbContext.AmsNeo4JNodeLabelRequiredRelations
                        .Include(x => x.Label)
                        .ThenInclude(x => x.ParentLabel)
                        .ThenInclude(x => x.Properties!)
                        .Include(x => x.Relation)
                        .ThenInclude(x => x.From)
                        .Include(x => x.Relation)
                        .ThenInclude(x => x.To)
                        .Include(x => x.Relation)
                        .ThenInclude(x => x.RelType)
                        .ToList();
                    return list;

                });
        }

        public List<AmsNeo4JNodeLabelRequiredRelation> GetLabelRequires(AmsNeo4JNodeLabel label)
        {
            return GetLabelRequires()
                .Where(x => x.LabelFk == label.Id)
                .ToList();
        }

        public void SaveLabelRequire(AmsNeo4JNodeLabelRequiredRelation require)
        {
            DbContext.AmsNeo4JNodeLabelRequiredRelations.Update(require);
            DbContext.SaveChanges();
            _cachProvider.Clear();
        }

        public IQueryable<AmsNeo4JProject>? GetProjects()
        {

            return _cachProvider.GetOrCreate(_cachProvider.CreateCachKey<AmsNeo4JProject>(nameof(GetProjects)),
                _ =>
                {

                    var list = DbContext.AmsNeo4JProjects.AsQueryable();
                    return list;
                });

        }

        public void SaveProject(AmsNeo4JProject project)
        {
            DbContext.AmsNeo4JProjects.Update(project);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(project);
        }

        public void SaveMicroservice(AmsNeo4JMicroservice microservice)
        {
            DbContext.AmsNeo4JMicroservices.Update(microservice);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(microservice);
        }

        public void SaveMicroserviceModule(AmsNeo4JMicroserviceModule microserviceModule)
        {
            DbContext.AmsNeo4JMicroserviceModules.Update(microserviceModule);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys(microserviceModule);
        }

        public IQueryable<AmsNeo4JMicroservice>? GetMiroservices(int projectFk)
        {
            return _cachProvider.GetOrCreate(
                _cachProvider.CreateCachKey<AmsNeo4JMicroservice>(nameof(GetMiroservices) + projectFk),
                _ =>
                {
                    return DbContext.AmsNeo4JMicroservices
                        //.AsNoTrackingWithIdentityResolution()
                        .Include(x => x.Project )
                        //.AsNoTrackingWithIdentityResolution()
                        .Where(x => projectFk == 0 || x.ProjectFk == projectFk)
                        //.ToList()
                        ;
                });
        }

        public IQueryable<AmsNeo4JMicroserviceModule>? GetMiroserviceModules(int msFk)
        {

            return _cachProvider.GetOrCreate(
                _cachProvider.CreateCachKey<AmsNeo4JMicroserviceModule>(nameof(GetMiroserviceModules) + msFk),
                _ =>
                {
                    return DbContext.AmsNeo4JMicroserviceModules
                        //.AsNoTrackingWithIdentityResolution()
                        .Include(x => x.Microservice)
                        .Where(x => msFk == 0 || x.MicroserviceFk == msFk)
                        //.ToList()
                        ;
                });
        }

        public void DeleteProject(AmsNeo4JProject prj)
        {
            DbContext.AmsNeo4JProjects.Remove(prj);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys();
        }

        public void DeleteMicroservice(AmsNeo4JMicroservice ms)
        {
            DbContext.AmsNeo4JMicroservices.Remove(ms);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys();
        }

        public void DeleteMicroserviceModule(AmsNeo4JMicroserviceModule msm)
        {
            DbContext.AmsNeo4JMicroserviceModules.Remove(msm);
            DbContext.SaveChanges();
            _cachProvider.ExpireCachKeys();
        }

        public void Dispose()
        {
            //DbContext.Dispose();
        }

        public List<AmsNeo4JMicroserviceModuleSetting>? GetLabelModuleSettings(AmsNeo4JNodeLabel label,
            AmsNeo4JMicroserviceModule? module)
        {
            return GetLabelModuleSettings(label.Id, module?.Id);
        }

        public List<AmsNeo4JMicroserviceModuleSetting>? GetLabelModuleSettings(int labelId, int? moduleId)
        {

            return _cachProvider.GetOrCreate(
                _cachProvider.CreateCachKey<AmsNeo4JMicroserviceModule>(nameof(GetLabelModuleSettings) + labelId+ "-" + moduleId),
                _ =>
                {
                    var xx= DbContext.AmsNeo4JMicroserviceModuleSettings
                        .Include(x => x.Label)
                        .Include(x => x.Module);
                    if (moduleId == null)
                    {
                        return xx
                            .Where(x => x.LabelFk == labelId )
                            .ToList();
                    }

                    return xx
                            .Where(x => x.LabelFk == labelId &&  x.MicroserviceModuleFk == moduleId)
                            .ToList()
                        ;
                });
        }

        public List<AmsNeo4JMicroserviceModuleSettingDefault>? GetMicroserviceModuleSettingDefaults(AmsNeo4JMicroserviceModule module)
        {
            return _cachProvider.GetOrCreate(
                _cachProvider.CreateCachKey<AmsNeo4JMicroserviceModuleSettingDefault>(nameof(GetMicroserviceModuleSettingDefaults) + module.Id),
                _ =>
                {
                    return DbContext.AmsNeo4JMicroserviceModuleSettingDefaults
                            //.AsNoTrackingWithIdentityResolution()
                            .Include(x => x.Module)
                            .Where(x => x.MicroserviceModuleFk == module.Id)
                            .ToList()
                        ;
                });

        }

        public void AddNewModuleSetting(AmsNeo4JMicroserviceModuleSetting setting)
        {

            DbContext.AmsNeo4JMicroserviceModuleSettings.Add(setting);
            DbContext.SaveChanges();
            ClearCach();
        }
        public void SaveModuleSetting(AmsNeo4JMicroserviceModuleSetting setting)
        {

            DbContext.AmsNeo4JMicroserviceModuleSettings.Update(setting);
            DbContext.SaveChanges();
            ClearCach();
        }

        public void SaveModuleSettingTemplate(AmsNeo4JMicroserviceModuleSettingTemplate setting)
        {

            DbContext.AmsNeo4JMicroserviceModuleSettingTemplates.Update(setting);
            DbContext.SaveChanges();
            ClearCach();
        }

        public List<AmsNeo4JMicroserviceModuleSettingTemplate> GetLabelModuleSettingTemplates()
        {
            return DbContext.AmsNeo4JMicroserviceModuleSettingTemplates.ToList();
        }

        public void DeleteModuleSettingTemplate(AmsNeo4JMicroserviceModuleSettingTemplate context)
        {
            DbContext.AmsNeo4JMicroserviceModuleSettingTemplates.Remove(context);
            DbContext.SaveChanges();
            ClearCach();
        }

        // public AmsNeo4JNodeLabelClassConfig? GetLabelClassConfig(AmsNeo4JNodeLabel? label)
        // {
        //     if(label == null) 
        //         return null;
        //
        //     var firstOrDefault = DbContext.AmsNeo4JNodeLabelClassConfigs.FirstOrDefault(x=>x.LabelFk == label.Id);
        //     return firstOrDefault;
        // }
        //
        // public void SaveLabelClassConfig(AmsNeo4JNodeLabelClassConfig classConf)
        // {
        //     DbContext.AmsNeo4JNodeLabelClassConfigs.Update(classConf);
        //     DbContext.SaveChanges();
        // }

        public IEnumerable<AmsNeo4JNodeLabelFunctionalId> GetFunctionalIds()
        {
             return DbContext.AmsNeo4JNodeLabelFunctionalIds.AsEnumerable(); 
        }

        /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
        /// <exception cref="DbUpdateConcurrencyException">A concurrency violation is encountered while saving to the database.
        ///                 A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///                 This is usually because the data in the database has been modified since it was loaded into memory.</exception>
        public void SaveFunctionalId(AmsNeo4JNodeLabelFunctionalId functionalId)
        {
            DbContext.AmsNeo4JNodeLabelFunctionalIds.Update(functionalId);
            DbContext.SaveChanges();


        }

        /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
        /// <exception cref="DbUpdateConcurrencyException">A concurrency violation is encountered while saving to the database.
        ///                 A concurrency violation occurs when an unexpected number of rows are affected during save.
        ///                 This is usually because the data in the database has been modified since it was loaded into memory.</exception>
        /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
        public async Task SaveChanges()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}
