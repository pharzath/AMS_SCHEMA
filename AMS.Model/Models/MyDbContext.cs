using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AMS.Model.Models;

public partial class MyDbContext : DbContext 
{
	private readonly IConfiguration m_Configuration;

    public MyDbContext(DbContextOptions<MyDbContext> options , IConfiguration configuration)
        : base(options)
    {
	    m_Configuration = configuration;
    }

    
    public virtual DbSet<AmsNeo4JMicroservice> AmsNeo4JMicroservices { get; set; }
    public virtual DbSet<AmsNeo4JMicroserviceModule> AmsNeo4JMicroserviceModules { get; set; }
    public virtual DbSet<AmsNeo4JMicroserviceModuleItemTemplateConfig> AmsNeo4JMicroserviceModuleItemTemplateConfigs { get; set; }
    public virtual DbSet<AmsNeo4JMicroserviceModuleSetting> AmsNeo4JMicroserviceModuleSettings { get; set; }
    public virtual DbSet<AmsNeo4JMicroserviceModuleSettingTemplate> AmsNeo4JMicroserviceModuleSettingTemplates { get; set; }
    public virtual DbSet<AmsNeo4JMicroserviceModuleSettingDefault> AmsNeo4JMicroserviceModuleSettingDefaults { get; set; }

    public virtual DbSet<AmsNeo4JDepartment> AmsNeo4JDepartments { get; set; }

    public virtual DbSet<AmsNeo4JNode> AmsNeo4JNodes { get; set; }

    public virtual DbSet<AmsNeo4JNodeConstraint> AmsNeo4JNodeConstraints { get; set; }

    public virtual DbSet<AmsNeo4JNodeDepartment> AmsNeo4JNodeDepartments { get; set; }

    public virtual DbSet<AmsNeo4JNodeIndex> AmsNeo4JNodeIndices { get; set; }

    public virtual DbSet<AmsNeo4JNodeLabel> AmsNeo4JNodeLabels { get; set; }
    //public virtual DbSet<AmsNeo4JNodeLabelClassConfig> AmsNeo4JNodeLabelClassConfigs { get; set; }
    public virtual DbSet<AmsNeo4JNodeLabelFunctionalId> AmsNeo4JNodeLabelFunctionalIds { get; set; }

    public virtual DbSet<AmsNeo4JNodeLabelProperty> AmsNeo4JNodeLabelProperties { get; set; }

    public virtual DbSet<AmsNeo4JNodeLabelQuery> AmsNeo4JNodeLabelQueries { get; set; }

    public virtual DbSet<AmsNeo4JNodeLabelRequiredRelation> AmsNeo4JNodeLabelRequiredRelations { get; set; }

    public virtual DbSet<AmsNeo4JNodeRelation> AmsNeo4JNodeRelations { get; set; }

    public virtual DbSet<AmsNeo4JNodeRelationProperty> AmsNeo4JNodeRelationProperties { get; set; }

    public virtual DbSet<AmsNeo4JNodeRelationType> AmsNeo4JNodeRelationTypes { get; set; }

    public virtual DbSet<AmsNeo4JProject> AmsNeo4JProjects { get; set; }

    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
	    // Get the connection string from the configuration file by its name
	    //var connectionString = m_Configuration.GetConnectionString("QOQNOSE_SCHEMA");
	    var connectionString = m_Configuration.GetConnectionString("AMS12");

		//optionsBuilder.UseSqlServer("Data Source=DESKTOP-1744E47\\MYSQLS;Initial Catalog=QOQNOSE_SCHEMA;User ID=sa;Password=baronix4353!;TrustServerCertificate=True");
		optionsBuilder.UseSqlServer(connectionString);
    }
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AS");

        modelBuilder.Entity<AmsNeo4JDepartment>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK_AMSModule_Department");

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
            entity.HasKey(e => e.NodeDepartmentId).HasName("PK_AMSModule_NodeDepartment");

            entity.ToTable("AMS_Neo4J_NodeDepartment");

            entity.Property(e => e.NodeDepartmentId).HasColumnName("NodeDepartmentID");
            entity.Property(e => e.CmsClassId)
                .HasDefaultValueSql("((0))")
                .HasColumnName("CmsClassID");
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
            entity.HasKey(e => e.Id).HasName("PK_AMS_Neo4J_NodeLable");

            entity.ToTable("AMS_Neo4J_NodeLabel");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Color).HasMaxLength(200);
            entity.Property(e => e.DisplayName).HasMaxLength(512);
            entity.Property(e => e.Icon).HasMaxLength(200);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasDefaultValueSql("(N'')");
            entity.Property(e => e.NodeId)
                .HasDefaultValueSql("((0))")
                .HasColumnName("NodeID");
            entity.Property(e => e.ParentLabelId).HasColumnName("ParentLabelID");
        });

        modelBuilder.Entity<AmsNeo4JNodeLabelProperty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AMS_Neo4J_NodeLabelPropery");

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
                .HasDefaultValueSql("((0))")
                .HasColumnName("FromFK");
            entity.Property(e => e.ToFk)
                .HasDefaultValueSql("((0))")
                .HasColumnName("ToFK");
            entity.Property(e => e.TypeFk).HasColumnName("TypeFK");
        });

        modelBuilder.Entity<AmsNeo4JNodeRelationProperty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AMS_Neo4J_NodeRelationPropery");

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
