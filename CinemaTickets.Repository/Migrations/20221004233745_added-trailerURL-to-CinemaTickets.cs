using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaTickets.Web.Data.Migrations
{
    public partial class addedtrailerURLtoCinemaTickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "trailerURL",
                table: "Films",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "trailerURL",
                table: "Films");
        }
    }
}
