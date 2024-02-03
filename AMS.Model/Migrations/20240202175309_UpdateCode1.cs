using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.Model.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCode1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.RenameColumn(
                name: "SubFolder",
                table: "AMS_Neo4J_Microservice_Module",
                newName: "FullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AMS_Neo4J_Microservice_Module",
                newName: "SubFolder");

            migrationBuilder.AddColumn<string>(
                name: "FullPath",
                table: "AMS_Neo4J_Microservice_Module",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
