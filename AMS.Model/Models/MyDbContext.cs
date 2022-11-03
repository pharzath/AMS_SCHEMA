using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AMS.Model.Models
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AmsNeo4JDepartment> AmsNeo4JDepartments { get; set; } = null!;
        public virtual DbSet<AmsNeo4JNode> AmsNeo4JNodes { get; set; } = null!;
        public virtual DbSet<AmsNeo4JNodeConstraint> AmsNeo4JNodeConstraints { get; set; } = null!;
        public virtual DbSet<AmsNeo4JNodeDepartment> AmsNeo4JNodeDepartments { get; set; } = null!;
        public virtual DbSet<AmsNeo4JNodeIndex> AmsNeo4JNodeIndices { get; set; } = null!;
        public virtual DbSet<AmsNeo4JNodeLabel> AmsNeo4JNodeLabels { get; set; } = null!;
        public virtual DbSet<AmsNeo4JNodeLabelProperty> AmsNeo4JNodeLabelProperties { get; set; } = null!;
        public virtual DbSet<AmsNeo4JNodeLabelQuery> AmsNeo4JNodeLabelQueries { get; set; } = null!;
        public virtual DbSet<AmsNeo4JNodeLabelRequiredRelation> AmsNeo4JNodeLabelRequiredRelations { get; set; } = null!;
        public virtual DbSet<AmsNeo4JNodeRelation> AmsNeo4JNodeRelations { get; set; } = null!;
        public virtual DbSet<AmsNeo4JNodeRelationProperty> AmsNeo4JNodeRelationProperties { get; set; } = null!;
        public virtual DbSet<AmsNeo4JNodeRelationType> AmsNeo4JNodeRelationTypes { get; set; } = null!;
        public virtual DbSet<AmsNeo4JProject> AmsNeo4JProjects { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=AMS12;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Latin1_General_CI_AS");

            modelBuilder.Entity<AmsNeo4JDepartment>(entity =>
            {
                entity.HasKey(e => e.DepartmentId)
                    .HasName("PK_AMSModule_Department");

                entity.ToTable("AMS_Neo4J_Department");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<AmsNeo4JNode>(entity =>
            {
                entity.ToTable("AMS_Neo4J_Node");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DisplayName).HasMaxLength(512);

                entity.Property(e => e.LabelId).HasColumnName("LabelID");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<AmsNeo4JNodeConstraint>(entity =>
            {
                entity.ToTable("AMS_Neo4J_NodeConstraints");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LabelFk).HasColumnName("LabelFK");

                entity.Property(e => e.Over).HasMaxLength(200);

                entity.Property(e => e.RelTypeFk).HasColumnName("RelTypeFK");

                entity.Property(e => e.Type).HasMaxLength(200);
            });

            modelBuilder.Entity<AmsNeo4JNodeDepartment>(entity =>
            {
                entity.HasKey(e => e.NodeDepartmentId)
                    .HasName("PK_AMSModule_NodeDepartment");

                entity.ToTable("AMS_Neo4J_NodeDepartment");

                entity.Property(e => e.NodeDepartmentId).HasColumnName("NodeDepartmentID");

                entity.Property(e => e.CmsClassId)
                    .HasColumnName("CmsClassID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.NodeId).HasColumnName("NodeID");
            });

            modelBuilder.Entity<AmsNeo4JNodeIndex>(entity =>
            {
                entity.ToTable("AMS_Neo4J_Node_Index");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LabelId).HasColumnName("LabelID");

                entity.Property(e => e.Over).HasMaxLength(200);

                entity.Property(e => e.RelTypeId).HasColumnName("RelTypeID");

                entity.Property(e => e.Type).HasMaxLength(200);
            });

            modelBuilder.Entity<AmsNeo4JNodeLabel>(entity =>
            {
                entity.ToTable("AMS_Neo4J_NodeLabel");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Color).HasMaxLength(200);

                entity.Property(e => e.DisplayName).HasMaxLength(512);

                entity.Property(e => e.Icon).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.NodeId)
                    .HasColumnName("NodeID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ParentLabelId).HasColumnName("ParentLabelID");
            });

            modelBuilder.Entity<AmsNeo4JNodeLabelProperty>(entity =>
            {
                entity.ToTable("AMS_Neo4J_NodeLabelProperty");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataType).HasMaxLength(200);

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.LabelId).HasColumnName("LabelID");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.Neo4jName).HasMaxLength(200);

                entity.Property(e => e.ValidationType).HasMaxLength(200);
            });

            modelBuilder.Entity<AmsNeo4JNodeLabelQuery>(entity =>
            {
                entity.ToTable("AMS_Neo4J_NodeLabelQuery");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DisplayName).HasMaxLength(512);

                entity.Property(e => e.LabelId).HasColumnName("LabelID");

                entity.Property(e => e.Name).HasMaxLength(512);
            });

            modelBuilder.Entity<AmsNeo4JNodeLabelRequiredRelation>(entity =>
            {
                entity.ToTable("AMS_Neo4J_NodeLabelRequiredRelation");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<AmsNeo4JNodeRelation>(entity =>
            {
                entity.ToTable("AMS_Neo4J_NodeRelation");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FromFk)
                    .HasColumnName("FromFK")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ToFk)
                    .HasColumnName("ToFK")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TypeFk).HasColumnName("TypeFK");
            });

            modelBuilder.Entity<AmsNeo4JNodeRelationProperty>(entity =>
            {
                entity.ToTable("AMS_Neo4J_NodeRelationProperty");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataType).HasMaxLength(200);

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Neo4jName).HasMaxLength(200);

                entity.Property(e => e.RelationId).HasColumnName("RelationID");

                entity.Property(e => e.ValidationType).HasMaxLength(200);
            });

            modelBuilder.Entity<AmsNeo4JNodeRelationType>(entity =>
            {
                entity.ToTable("AMS_Neo4J_Node_RelationType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DisplayName).HasMaxLength(512);

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<AmsNeo4JProject>(entity =>
            {
                entity.ToTable("AMS_Neo4J_Project");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DisplayName).HasMaxLength(512);

                entity.Property(e => e.Name).HasMaxLength(512);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
