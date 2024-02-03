﻿// <auto-generated />
using System;
using AMS.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AMS.Model.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20240203210908_Update4")]
    partial class Update4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("Latin1_General_CI_AS")
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AMS.Model.AmsNeo4JMicroservice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Folder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectFk")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectFk");

                    b.ToTable("AMS_Neo4J_Microservice");
                });

            modelBuilder.Entity("AMS.Model.AmsNeo4JMicroserviceModule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MicroserviceFk")
                        .HasColumnType("int");

                    b.Property<int>("ModuleType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SortOrder")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MicroserviceFk");

                    b.ToTable("AMS_Neo4J_Microservice_Module");
                });

            modelBuilder.Entity("AMS.Model.AmsNeo4JMicroserviceModuleItemTemplateConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FileSystemItemId")
                        .HasColumnType("int");

                    b.Property<bool>("Ignored")
                        .HasColumnType("bit");

                    b.Property<int>("ModuleId")
                        .HasColumnType("int");

                    b.Property<string>("NamePolicy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FileSystemItemId");

                    b.HasIndex("ModuleId");

                    b.ToTable("AmsNeo4JMicroserviceModuleItemTemplateConfigs");
                });

            modelBuilder.Entity("AMS.Model.AmsNeo4JMicroserviceModuleSetting", b =>
                {
                    b.Property<int>("MicroserviceModuleFk")
                        .HasColumnType("int");

                    b.Property<int>("LabelFk")
                        .HasColumnType("int");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("CopiedFromModuleFk")
                        .HasColumnType("int");

                    b.Property<string>("DataValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SettingType")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MicroserviceModuleFk", "LabelFk", "Key");

                    b.HasIndex("CopiedFromModuleFk");

                    b.HasIndex("LabelFk");

                    b.ToTable("AMS_Neo4J_Microservice_Module_Setting");
                });

            modelBuilder.Entity("AMS.Model.AmsNeo4JMicroserviceModuleSettingDefault", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DefaultKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DefaultValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MicroserviceModuleFk")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MicroserviceModuleFk");

                    b.ToTable("AMS_Neo4J_Microservice_Module_Setting_Default");
                });

            modelBuilder.Entity("AMS.Model.AmsNeo4JMicroserviceModuleSettingTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DataValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SettingType")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AMS_Neo4J_Microservice_Module_Setting_Template");
                });

            modelBuilder.Entity("AMS.Model.FileSystemItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("FileSystemItemId")
                        .HasColumnType("int");

                    b.Property<string>("FullPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsExpanded")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FileSystemItemId");

                    b.ToTable("FileSystemItem");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JDepartment", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DepartmentID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentId"));

                    b.Property<int?>("ProjectFk")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("DepartmentId")
                        .HasName("PK_AMSModule_Department");

                    b.HasIndex("ProjectFk");

                    b.ToTable("AMS_Neo4J_Department", (string)null);
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DisplayName")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<int?>("LabelId")
                        .HasColumnType("int")
                        .HasColumnName("LabelID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasDefaultValueSql("(N'')");

                    b.Property<int?>("ProjectFk")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LabelId");

                    b.ToTable("AMS_Neo4J_Node", (string)null);
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeConstraint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Command")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LabelFk")
                        .HasColumnType("int")
                        .HasColumnName("LabelFK");

                    b.Property<string>("Over")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("RelTypeFk")
                        .HasColumnType("int")
                        .HasColumnName("RelTypeFK");

                    b.Property<string>("Type")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("LabelFk");

                    b.HasIndex("RelTypeFk");

                    b.ToTable("AMS_Neo4J_NodeConstraints", (string)null);
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeDepartment", b =>
                {
                    b.Property<int>("NodeDepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("NodeDepartmentID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NodeDepartmentId"));

                    b.Property<int?>("CmsClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CmsClassID")
                        .HasDefaultValueSql("((0))");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int")
                        .HasColumnName("DepartmentID");

                    b.Property<int?>("NodeId")
                        .HasColumnType("int")
                        .HasColumnName("NodeID");

                    b.HasKey("NodeDepartmentId")
                        .HasName("PK_AMSModule_NodeDepartment");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("NodeId");

                    b.ToTable("AMS_Neo4J_NodeDepartment", (string)null);
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeIndex", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Command")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LabelId")
                        .HasColumnType("int")
                        .HasColumnName("LabelID");

                    b.Property<string>("Over")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("RelTypeId")
                        .HasColumnType("int")
                        .HasColumnName("RelTypeID");

                    b.Property<string>("Type")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("LabelId");

                    b.HasIndex("RelTypeId");

                    b.ToTable("AMS_Neo4J_Node_Index", (string)null);
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeLabel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<int?>("FunctionalIdFk")
                        .HasColumnType("int");

                    b.Property<string>("Icon")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("IconContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("InheritPropsFromParentLabel")
                        .HasColumnType("bit");

                    b.Property<int?>("InheriteFromLabelId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsAbstract")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsStaticData")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsVirtual")
                        .HasColumnType("bit");

                    b.Property<int?>("MicroserviceFk")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasDefaultValueSql("(N'')");

                    b.Property<int?>("NodeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("NodeID")
                        .HasDefaultValueSql("((0))");

                    b.Property<int?>("ParentLabelId")
                        .HasColumnType("int")
                        .HasColumnName("ParentLabelID");

                    b.HasKey("Id")
                        .HasName("PK_AMS_Neo4J_NodeLable");

                    b.HasIndex("FunctionalIdFk");

                    b.HasIndex("InheriteFromLabelId");

                    b.HasIndex("MicroserviceFk");

                    b.HasIndex("NodeId");

                    b.HasIndex("ParentLabelId");

                    b.ToTable("AMS_Neo4J_NodeLabel", (string)null);
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeLabelFunctionalId", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prefix")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Ams_Neo4J_Node_Label_FunctionalId");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeLabelProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("AllowNull")
                        .HasColumnType("bit");

                    b.Property<bool?>("AlternateGrainKey")
                        .HasColumnType("bit");

                    b.Property<string>("DataType")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ItemsStringValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LabelId")
                        .HasColumnType("int")
                        .HasColumnName("LabelID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasDefaultValueSql("(N'')");

                    b.Property<string>("Neo4jName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<string>("ValidationType")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id")
                        .HasName("PK_AMS_Neo4J_NodeLabelPropery");

                    b.HasIndex("LabelId");

                    b.ToTable("AMS_Neo4J_NodeLabelProperty", (string)null);
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeLabelQuery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CypherQuery")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<int?>("LabelId")
                        .HasColumnType("int")
                        .HasColumnName("LabelID");

                    b.Property<string>("Name")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("RelatedLabels")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LabelId");

                    b.ToTable("AMS_Neo4J_NodeLabelQuery", (string)null);
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeLabelRequiredRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsForceToCreateNew")
                        .HasColumnType("bit");

                    b.Property<int>("LabelFk")
                        .HasColumnType("int");

                    b.Property<int>("LabelRelationFk")
                        .HasColumnType("int");

                    b.Property<int>("ReqCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LabelFk");

                    b.HasIndex("LabelRelationFk");

                    b.ToTable("AMS_Neo4J_NodeLabelRequiredRelation", (string)null);
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FromFk")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("FromFK")
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("InEntPropName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InEntPropType")
                        .HasColumnType("int");

                    b.Property<bool>("IsRequiredRelation")
                        .HasColumnType("bit");

                    b.Property<string>("OutEntPropName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OutEntPropType")
                        .HasColumnType("int");

                    b.Property<int?>("ToFk")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ToFK")
                        .HasDefaultValueSql("((0))");

                    b.Property<int?>("TypeFk")
                        .HasColumnType("int")
                        .HasColumnName("TypeFK");

                    b.HasKey("Id");

                    b.HasIndex("FromFk");

                    b.HasIndex("ToFk");

                    b.HasIndex("TypeFk");

                    b.ToTable("AMS_Neo4J_NodeRelation", (string)null);
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeRelationProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DataType")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Neo4jName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<int?>("RelationId")
                        .HasColumnType("int")
                        .HasColumnName("RelationID");

                    b.Property<string>("ValidationType")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id")
                        .HasName("PK_AMS_Neo4J_NodeRelationPropery");

                    b.HasIndex("RelationId");

                    b.ToTable("AMS_Neo4J_NodeRelationProperty", (string)null);
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeRelationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DisplayName")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("AMS_Neo4J_Node_RelationType", (string)null);
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JProject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("MicroservicesPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Namespace")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RootPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AMS_Neo4J_Project", (string)null);
                });

            modelBuilder.Entity("AMS.Model.AmsNeo4JMicroservice", b =>
                {
                    b.HasOne("AMS.Model.Models.AmsNeo4JProject", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectFk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("AMS.Model.AmsNeo4JMicroserviceModule", b =>
                {
                    b.HasOne("AMS.Model.AmsNeo4JMicroservice", "Microservice")
                        .WithMany("Modules")
                        .HasForeignKey("MicroserviceFk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Microservice");
                });

            modelBuilder.Entity("AMS.Model.AmsNeo4JMicroserviceModuleItemTemplateConfig", b =>
                {
                    b.HasOne("AMS.Model.FileSystemItem", "FileSystemItem")
                        .WithMany()
                        .HasForeignKey("FileSystemItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AMS.Model.AmsNeo4JMicroserviceModule", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FileSystemItem");

                    b.Navigation("Module");
                });

            modelBuilder.Entity("AMS.Model.AmsNeo4JMicroserviceModuleSetting", b =>
                {
                    b.HasOne("AMS.Model.AmsNeo4JMicroserviceModule", "CopiedFromModule")
                        .WithMany()
                        .HasForeignKey("CopiedFromModuleFk");

                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeLabel", "Label")
                        .WithMany("ModuleSettings")
                        .HasForeignKey("LabelFk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AMS.Model.AmsNeo4JMicroserviceModule", "Module")
                        .WithMany()
                        .HasForeignKey("MicroserviceModuleFk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CopiedFromModule");

                    b.Navigation("Label");

                    b.Navigation("Module");
                });

            modelBuilder.Entity("AMS.Model.AmsNeo4JMicroserviceModuleSettingDefault", b =>
                {
                    b.HasOne("AMS.Model.AmsNeo4JMicroserviceModule", "Module")
                        .WithMany()
                        .HasForeignKey("MicroserviceModuleFk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Module");
                });

            modelBuilder.Entity("AMS.Model.FileSystemItem", b =>
                {
                    b.HasOne("AMS.Model.FileSystemItem", null)
                        .WithMany("Children")
                        .HasForeignKey("FileSystemItemId");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JDepartment", b =>
                {
                    b.HasOne("AMS.Model.Models.AmsNeo4JProject", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectFk");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNode", b =>
                {
                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeLabel", "Label")
                        .WithMany()
                        .HasForeignKey("LabelId");

                    b.Navigation("Label");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeConstraint", b =>
                {
                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeLabel", "Label")
                        .WithMany("Constraints")
                        .HasForeignKey("LabelFk");

                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeRelationType", "RelType")
                        .WithMany("Constraints")
                        .HasForeignKey("RelTypeFk");

                    b.Navigation("Label");

                    b.Navigation("RelType");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeDepartment", b =>
                {
                    b.HasOne("AMS.Model.Models.AmsNeo4JDepartment", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");

                    b.HasOne("AMS.Model.Models.AmsNeo4JNode", "Node")
                        .WithMany("Departments")
                        .HasForeignKey("NodeId");

                    b.Navigation("Department");

                    b.Navigation("Node");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeIndex", b =>
                {
                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeLabel", "Label")
                        .WithMany("Indices")
                        .HasForeignKey("LabelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeRelationType", "RelType")
                        .WithMany("Indices")
                        .HasForeignKey("RelTypeId");

                    b.Navigation("Label");

                    b.Navigation("RelType");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeLabel", b =>
                {
                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeLabelFunctionalId", "FunctionalId")
                        .WithMany()
                        .HasForeignKey("FunctionalIdFk");

                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeLabel", "InheriteFromLabel")
                        .WithMany()
                        .HasForeignKey("InheriteFromLabelId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AMS.Model.AmsNeo4JMicroservice", "Microservice")
                        .WithMany()
                        .HasForeignKey("MicroserviceFk");

                    b.HasOne("AMS.Model.Models.AmsNeo4JNode", "Node")
                        .WithMany()
                        .HasForeignKey("NodeId");

                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeLabel", "ParentLabel")
                        .WithMany("ChildLabels")
                        .HasForeignKey("ParentLabelId");

                    b.Navigation("FunctionalId");

                    b.Navigation("InheriteFromLabel");

                    b.Navigation("Microservice");

                    b.Navigation("Node");

                    b.Navigation("ParentLabel");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeLabelProperty", b =>
                {
                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeLabel", "Label")
                        .WithMany("Properties")
                        .HasForeignKey("LabelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Label");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeLabelQuery", b =>
                {
                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeLabel", "Label")
                        .WithMany()
                        .HasForeignKey("LabelId");

                    b.Navigation("Label");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeLabelRequiredRelation", b =>
                {
                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeLabel", "Label")
                        .WithMany()
                        .HasForeignKey("LabelFk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeRelation", "Relation")
                        .WithMany()
                        .HasForeignKey("LabelRelationFk")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Label");

                    b.Navigation("Relation");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeRelation", b =>
                {
                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeLabel", "From")
                        .WithMany()
                        .HasForeignKey("FromFk");

                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeLabel", "To")
                        .WithMany()
                        .HasForeignKey("ToFk");

                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeRelationType", "RelType")
                        .WithMany()
                        .HasForeignKey("TypeFk");

                    b.Navigation("From");

                    b.Navigation("RelType");

                    b.Navigation("To");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeRelationProperty", b =>
                {
                    b.HasOne("AMS.Model.Models.AmsNeo4JNodeRelationType", "RelType")
                        .WithMany("Properties")
                        .HasForeignKey("RelationId");

                    b.Navigation("RelType");
                });

            modelBuilder.Entity("AMS.Model.AmsNeo4JMicroservice", b =>
                {
                    b.Navigation("Modules");
                });

            modelBuilder.Entity("AMS.Model.FileSystemItem", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNode", b =>
                {
                    b.Navigation("Departments");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeLabel", b =>
                {
                    b.Navigation("ChildLabels");

                    b.Navigation("Constraints");

                    b.Navigation("Indices");

                    b.Navigation("ModuleSettings");

                    b.Navigation("Properties");
                });

            modelBuilder.Entity("AMS.Model.Models.AmsNeo4JNodeRelationType", b =>
                {
                    b.Navigation("Constraints");

                    b.Navigation("Indices");

                    b.Navigation("Properties");
                });
#pragma warning restore 612, 618
        }
    }
}
