using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NecManager.Server.DataAccessLayer.Migrations
{
    public partial class UpdateStudentBirthdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Students");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
