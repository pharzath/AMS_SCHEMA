using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.Model.Migrations
{
    /// <inheritdoc />
    public partial class MIG1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "AMS_Neo4J_Node",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "AMS_Neo4J_Node");
        }
    }
}
