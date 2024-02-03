using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.Model.Migrations
{
    /// <inheritdoc />
    public partial class Update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileSystemItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsExpanded = table.Column<bool>(type: "bit", nullable: false),
                    FileSystemItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileSystemItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileSystemItem_FileSystemItem_FileSystemItemId",
                        column: x => x.FileSystemItemId,
                        principalTable: "FileSystemItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AmsNeo4JMicroserviceModuleItemTemplateConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    FileSystemItemId = table.Column<int>(type: "int", nullable: false),
                    Ignored = table.Column<bool>(type: "bit", nullable: false),
                    NamePolicy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmsNeo4JMicroserviceModuleItemTemplateConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmsNeo4JMicroserviceModuleItemTemplateConfigs_AMS_Neo4J_Microservice_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "AMS_Neo4J_Microservice_Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AmsNeo4JMicroserviceModuleItemTemplateConfigs_FileSystemItem_FileSystemItemId",
                        column: x => x.FileSystemItemId,
                        principalTable: "FileSystemItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AmsNeo4JMicroserviceModuleItemTemplateConfigs_FileSystemItemId",
                table: "AmsNeo4JMicroserviceModuleItemTemplateConfigs",
                column: "FileSystemItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AmsNeo4JMicroserviceModuleItemTemplateConfigs_ModuleId",
                table: "AmsNeo4JMicroserviceModuleItemTemplateConfigs",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_FileSystemItem_FileSystemItemId",
                table: "FileSystemItem",
                column: "FileSystemItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmsNeo4JMicroserviceModuleItemTemplateConfigs");

            migrationBuilder.DropTable(
                name: "FileSystemItem");
        }
    }
}
