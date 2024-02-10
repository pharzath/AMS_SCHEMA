using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleToFileSystemItem4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "AmsNeo4JMicroserviceModuleItemTemplateConfigs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "AmsNeo4JMicroserviceModuleItemTemplateConfigs");
        }
    }
}
