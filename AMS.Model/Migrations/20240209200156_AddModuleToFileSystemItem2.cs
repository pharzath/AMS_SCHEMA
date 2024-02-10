using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleToFileSystemItem2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RelativePath",
                table: "FileSystemItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelativePath",
                table: "FileSystemItem");
        }
    }
}
