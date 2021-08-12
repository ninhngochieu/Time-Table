using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTableBackend.Migrations
{
    public partial class ThemMonHoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonHocs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ten = table.Column<string>(type: "TEXT", nullable: true),
                    MaMonHoc = table.Column<string>(type: "TEXT", nullable: true),
                    SoTinChi = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonHocs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonHocs");
        }
    }
}
