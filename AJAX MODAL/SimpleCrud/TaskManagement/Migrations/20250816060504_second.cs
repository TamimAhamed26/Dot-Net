using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "AssignedTasks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmitDate",
                table: "AssignedTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "AssignedTasks");

            migrationBuilder.DropColumn(
                name: "SubmitDate",
                table: "AssignedTasks");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Tasks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
