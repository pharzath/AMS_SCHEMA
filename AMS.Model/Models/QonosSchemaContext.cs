using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AMS.Model.Models;

public partial class QonosSchemaContext : DbContext
{
    public QonosSchemaContext()
    {
    }

    public QonosSchemaContext(DbContextOptions<QonosSchemaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AmsNeo4JDepartment> AmsNeo4JDepartments { get; set; }

    public virtual DbSet<AmsNeo4JNode> AmsNeo4JNodes { get; set; }

    public virtual DbSet<AmsNeo4JNodeConstraint> AmsNeo4JNodeConstraints { get; set; }

    public virtual DbSet<AmsNeo4JNodeDepartment> AmsNeo4JNodeDepartments { get; set; }

    public virtual DbSet<AmsNeo4JNodeIndex> AmsNeo4JNodeIndices { get; set; }

    public virtual DbSet<AmsNeo4JNodeLabel> AmsNeo4JNodeLabels { get; set; }

    public virtual DbSet<AmsNeo4JNodeLabelProperty> AmsNeo4JNodeLabelProperties { get; set; }

    public virtual DbSet<AmsNeo4JNodeLabelQuery> AmsNeo4JNodeLabelQueries { get; set; }

    public virtual DbSet<AmsNeo4JNodeLabelRequiredRelation> AmsNeo4JNodeLabelRequiredRelations { get; set; }

    public virtual DbSet<AmsNeo4JNodeRelation> AmsNeo4JNodeRelations { get; set; }

    public virtual DbSet<AmsNeo4JNodeRelationProperty> AmsNeo4JNodeRelationProperties { get; set; }

    public virtual DbSet<AmsNeo4JNodeRelationType> AmsNeo4JNodeRelationTypes { get; set; }

    public virtual DbSet<AmsNeo4JProject> AmsNeo4JProjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=E:\\PROJ\\AMS\\AMSCHEMA\\AMS_SCHEMA\\AMS.Model\\SqliteDb\\QonosSchema.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AmsNeo4JDepartment>(entity =>
        {
            entity.HasKey(e => e.DepartmentId);

            entity.ToTable("AMS_Neo4J_Department");

            entity.Property(e => e.DepartmentId)
                .ValueGeneratedNever()
                .HasColumnName("DepartmentID");
            entity.Property(e => e.Title).HasColumnType("text(200)");
        });

        modelBuilder.Entity<AmsNeo4JNode>(entity =>
        {
            entity.ToTable("AMS_Neo4J_Node");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DisplayName).HasColumnType("text(512)");
            entity.Property(e => e.LabelId).HasColumnName("LabelID");
            entity.Property(e => e.Name).HasColumnType("text(200)");
        });

        modelBuilder.Entity<AmsNeo4JNodeConstraint>(entity =>
        {
            entity.ToTable("AMS_Neo4J_NodeConstraints");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.LabelFk).HasColumnName("LabelFK");
            entity.Property(e => e.Over).HasColumnType("text(200)");
            entity.Property(e => e.RelTypeFk).HasColumnName("RelTypeFK");
            entity.Property(e => e.Type).HasColumnType("text(200)");
        });

        modelBuilder.Entity<AmsNeo4JNodeDepartment>(entity =>
        {
            entity.HasKey(e => e.NodeDepartmentId);

            entity.ToTable("AMS_Neo4J_NodeDepartment");

            entity.Property(e => e.NodeDepartmentId)
                .ValueGeneratedNever()
                .HasColumnName("NodeDepartmentID");
            entity.Property(e => e.CmsClassId).HasColumnName("CmsClassID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.NodeId).HasColumnName("NodeID");
        });

        modelBuilder.Entity<AmsNeo4JNodeIndex>(entity =>
        {
            entity.ToTable("AMS_Neo4J_Node_Index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.LabelId).HasColumnName("LabelID");
            entity.Property(e => e.Over).HasColumnType("text(200)");
            entity.Property(e => e.RelTypeId).HasColumnName("RelTypeID");
            entity.Property(e => e.Type).HasColumnType("text(200)");
        });

        modelBuilder.Entity<AmsNeo4JNodeLabel>(entity =>
        {
            entity.ToTable("AMS_Neo4J_NodeLabel");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Color).HasColumnType("text(200)");
            entity.Property(e => e.DisplayName).HasColumnType("text(512)");
            entity.Property(e => e.FunctionalId).HasColumnType("text(36)");
            entity.Property(e => e.Guid).HasColumnType("text(36)");
            entity.Property(e => e.Icon).HasColumnType("text(200)");
            entity.Property(e => e.Name).HasColumnType("text(200)");
            entity.Property(e => e.NodeId).HasColumnName("NodeID");
            entity.Property(e => e.ParentLabelId).HasColumnName("ParentLabelID");
        });

        modelBuilder.Entity<AmsNeo4JNodeLabelProperty>(entity =>
        {
            entity.ToTable("AMS_Neo4J_NodeLabelProperty");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DataType).HasColumnType("text(200)");
            entity.Property(e => e.DisplayName).HasColumnType("text(200)");
            entity.Property(e => e.Guid).HasColumnType("text(36)");
            entity.Property(e => e.LabelId).HasColumnName("LabelID");
            entity.Property(e => e.Name).HasColumnType("text(200)");
            entity.Property(e => e.Neo4jName).HasColumnType("text(200)");
            entity.Property(e => e.ValidationType).HasColumnType("text(200)");
        });

        modelBuilder.Entity<AmsNeo4JNodeLabelQuery>(entity =>
        {
            entity.ToTable("AMS_Neo4J_NodeLabelQuery");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DisplayName).HasColumnType("text(512)");
            entity.Property(e => e.LabelId).HasColumnName("LabelID");
            entity.Property(e => e.Name).HasColumnType("text(512)");
        });

        modelBuilder.Entity<AmsNeo4JNodeLabelRequiredRelation>(entity =>
        {
            entity.ToTable("AMS_Neo4J_NodeLabelRequiredRelation");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<AmsNeo4JNodeRelation>(entity =>
        {
            entity.ToTable("AMS_Neo4J_NodeRelation");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.FromFk).HasColumnName("FromFK");
            entity.Property(e => e.Guid).HasColumnType("text(36)");
            entity.Property(e => e.ToFk).HasColumnName("ToFK");
            entity.Property(e => e.TypeFk).HasColumnName("TypeFK");
        });

        modelBuilder.Entity<AmsNeo4JNodeRelationProperty>(entity =>
        {
            entity.ToTable("AMS_Neo4J_NodeRelationProperty");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DataType).HasColumnType("text(200)");
            entity.Property(e => e.DisplayName).HasColumnType("text(200)");
            entity.Property(e => e.Guid).HasColumnType("text(36)");
            entity.Property(e => e.Name).HasColumnType("text(200)");
            entity.Property(e => e.Neo4jName).HasColumnType("text(200)");
            entity.Property(e => e.RelationId).HasColumnName("RelationID");
            entity.Property(e => e.ValidationType).HasColumnType("text(200)");
        });

        modelBuilder.Entity<AmsNeo4JNodeRelationType>(entity =>
        {
            entity.ToTable("AMS_Neo4J_Node_RelationType");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DisplayName).HasColumnType("text(512)");
            entity.Property(e => e.Name).HasColumnType("text(200)");
        });

        modelBuilder.Entity<AmsNeo4JProject>(entity =>
        {
            entity.ToTable("AMS_Neo4J_Project");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DisplayName).HasColumnType("text(512)");
            entity.Property(e => e.Guid).HasColumnType("text(36)");
            entity.Property(e => e.Name).HasColumnType("text(512)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
