using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleToFileSystemItem3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelativePath",
                table: "AmsNeo4JMicroserviceModuleItemTemplateConfigs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RelativePath",
                table: "AmsNeo4JMicroserviceModuleItemTemplateConfigs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
