using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NecManager.Server.DataAccessLayer.Migrations
{
    public partial class UpdateTournamentEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChampionId",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rank2Id",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(    
                name: "Rank3Id",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rank4Id",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rank5Id",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChampionId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Rank2Id",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Rank3Id",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Rank4Id",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Rank5Id",
                table: "Tournaments");
        }
    }
}
