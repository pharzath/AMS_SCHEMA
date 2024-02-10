using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleToFileSystemItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeneratorTemplatePath",
                table: "AmsNeo4JMicroserviceModuleItemTemplateConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelativePath",
                table: "AmsNeo4JMicroserviceModuleItemTemplateConfigs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneratorTemplatePath",
                table: "AmsNeo4JMicroserviceModuleItemTemplateConfigs");

            migrationBuilder.DropColumn(
                name: "RelativePath",
                table: "AmsNeo4JMicroserviceModuleItemTemplateConfigs");
        }
    }
}
