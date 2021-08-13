using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTableBackend.Migrations
{
    public partial class BuoiThemGiangVienFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TietKetThuc",
                table: "Buois");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TietKetThuc",
                table: "Buois",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
