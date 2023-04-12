using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NecManager.Server.DataAccessLayer.Migrations
{
    public partial class AddStudentState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Students");
        }
    }
}
