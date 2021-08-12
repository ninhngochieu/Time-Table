using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTableBackend.Migrations
{
    public partial class ThemNhomMonHoc : Migration
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

            migrationBuilder.CreateTable(
                name: "NhomMonHocs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NMH = table.Column<string>(type: "TEXT", nullable: true),
                    Phong = table.Column<string>(type: "TEXT", nullable: true),
                    MonHocId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhomMonHocs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NhomMonHocs_MonHocs_MonHocId",
                        column: x => x.MonHocId,
                        principalTable: "MonHocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NhomMonHocs_MonHocId",
                table: "NhomMonHocs",
                column: "MonHocId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NhomMonHocs");

            migrationBuilder.DropTable(
                name: "MonHocs");
        }
    }
}
