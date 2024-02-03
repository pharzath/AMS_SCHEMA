using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMS.Model.Migrations
{
    /// <inheritdoc />
    public partial class SortOrderMsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AMS_Neo4J_Node_AMS_Neo4J_NodeLabel_LabelID",
                table: "AMS_Neo4J_Node");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_AMS_Neo4J_Node_Index_AMS_Neo4J_Node_RelationType_RelTypeID",
            //     table: "AMS_Neo4J_Node_Index");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_AMS_Neo4J_NodeDepartment_AMS_Neo4J_Department_DepartmentID",
            //     table: "AMS_Neo4J_NodeDepartment");
            //
            // migrationBuilder.DropForeignKey(
            //     name: "FK_AMS_Neo4J_NodeDepartment_AMS_Neo4J_Node_NodeID",
            //     table: "AMS_Neo4J_NodeDepartment");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_AMS_Neo4J_NodeLabelQuery_AMS_Neo4J_NodeLabel_LabelID",
            //     table: "AMS_Neo4J_NodeLabelQuery");

            migrationBuilder.AlterColumn<int>(
                name: "LabelID",
                table: "AMS_Neo4J_NodeLabelQuery",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "NodeID",
                table: "AMS_Neo4J_NodeDepartment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "AMS_Neo4J_NodeDepartment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RelTypeID",
                table: "AMS_Neo4J_Node_Index",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LabelID",
                table: "AMS_Neo4J_Node",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "AMS_Neo4J_Microservice_Module",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AMS_Neo4J_Node_AMS_Neo4J_NodeLabel_LabelID",
                table: "AMS_Neo4J_Node",
                column: "LabelID",
                principalTable: "AMS_Neo4J_NodeLabel",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AMS_Neo4J_Node_Index_AMS_Neo4J_Node_RelationType_RelTypeID",
                table: "AMS_Neo4J_Node_Index",
                column: "RelTypeID",
                principalTable: "AMS_Neo4J_Node_RelationType",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AMS_Neo4J_NodeDepartment_AMS_Neo4J_Department_DepartmentID",
                table: "AMS_Neo4J_NodeDepartment",
                column: "DepartmentID",
                principalTable: "AMS_Neo4J_Department",
                principalColumn: "DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_AMS_Neo4J_NodeDepartment_AMS_Neo4J_Node_NodeID",
                table: "AMS_Neo4J_NodeDepartment",
                column: "NodeID",
                principalTable: "AMS_Neo4J_Node",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AMS_Neo4J_NodeLabelQuery_AMS_Neo4J_NodeLabel_LabelID",
                table: "AMS_Neo4J_NodeLabelQuery",
                column: "LabelID",
                principalTable: "AMS_Neo4J_NodeLabel",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AMS_Neo4J_Node_AMS_Neo4J_NodeLabel_LabelID",
                table: "AMS_Neo4J_Node");

            migrationBuilder.DropForeignKey(
                name: "FK_AMS_Neo4J_Node_Index_AMS_Neo4J_Node_RelationType_RelTypeID",
                table: "AMS_Neo4J_Node_Index");

            migrationBuilder.DropForeignKey(
                name: "FK_AMS_Neo4J_NodeDepartment_AMS_Neo4J_Department_DepartmentID",
                table: "AMS_Neo4J_NodeDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_AMS_Neo4J_NodeDepartment_AMS_Neo4J_Node_NodeID",
                table: "AMS_Neo4J_NodeDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_AMS_Neo4J_NodeLabelQuery_AMS_Neo4J_NodeLabel_LabelID",
                table: "AMS_Neo4J_NodeLabelQuery");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "AMS_Neo4J_Microservice_Module");

            migrationBuilder.AlterColumn<int>(
                name: "LabelID",
                table: "AMS_Neo4J_NodeLabelQuery",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NodeID",
                table: "AMS_Neo4J_NodeDepartment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "AMS_Neo4J_NodeDepartment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RelTypeID",
                table: "AMS_Neo4J_Node_Index",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LabelID",
                table: "AMS_Neo4J_Node",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AMS_Neo4J_Node_AMS_Neo4J_NodeLabel_LabelID",
                table: "AMS_Neo4J_Node",
                column: "LabelID",
                principalTable: "AMS_Neo4J_NodeLabel",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AMS_Neo4J_Node_Index_AMS_Neo4J_Node_RelationType_RelTypeID",
                table: "AMS_Neo4J_Node_Index",
                column: "RelTypeID",
                principalTable: "AMS_Neo4J_Node_RelationType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AMS_Neo4J_NodeDepartment_AMS_Neo4J_Department_DepartmentID",
                table: "AMS_Neo4J_NodeDepartment",
                column: "DepartmentID",
                principalTable: "AMS_Neo4J_Department",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AMS_Neo4J_NodeDepartment_AMS_Neo4J_Node_NodeID",
                table: "AMS_Neo4J_NodeDepartment",
                column: "NodeID",
                principalTable: "AMS_Neo4J_Node",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AMS_Neo4J_NodeLabelQuery_AMS_Neo4J_NodeLabel_LabelID",
                table: "AMS_Neo4J_NodeLabelQuery",
                column: "LabelID",
                principalTable: "AMS_Neo4J_NodeLabel",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
