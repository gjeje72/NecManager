using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NecManager.Server.DataAccessLayer.Migrations
{
    public partial class UpdateTrainingMasterName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MasterName",
                table: "PersonTrainings");

            migrationBuilder.AddColumn<string>(
                name: "MasterName",
                table: "Trainings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MasterName",
                table: "Trainings");

            migrationBuilder.AddColumn<string>(
                name: "MasterName",
                table: "PersonTrainings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
