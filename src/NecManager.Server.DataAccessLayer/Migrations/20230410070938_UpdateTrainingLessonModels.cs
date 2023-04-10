using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NecManager.Server.DataAccessLayer.Migrations
{
    public partial class UpdateTrainingLessonModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Trainings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MasterName",
                table: "PersonTrainings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_GroupId",
                table: "Trainings",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Groups_GroupId",
                table: "Trainings",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Groups_GroupId",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_GroupId",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "MasterName",
                table: "PersonTrainings");
        }
    }
}
