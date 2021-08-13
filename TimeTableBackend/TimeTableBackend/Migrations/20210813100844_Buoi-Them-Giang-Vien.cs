using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTableBackend.Migrations
{
    public partial class BuoiThemGiangVien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GiangVien",
                table: "Buois",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiangVien",
                table: "Buois");
        }
    }
}
