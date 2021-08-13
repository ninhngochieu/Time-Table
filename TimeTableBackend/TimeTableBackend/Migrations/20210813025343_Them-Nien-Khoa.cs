using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTableBackend.Migrations
{
    public partial class ThemNienKhoa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NienKhoaId",
                table: "MonHocs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NienKhoas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HocKy = table.Column<string>(type: "TEXT", nullable: true),
                    NamHoc = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NienKhoas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonHocs_NienKhoaId",
                table: "MonHocs",
                column: "NienKhoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonHocs_NienKhoas_NienKhoaId",
                table: "MonHocs",
                column: "NienKhoaId",
                principalTable: "NienKhoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonHocs_NienKhoas_NienKhoaId",
                table: "MonHocs");

            migrationBuilder.DropTable(
                name: "NienKhoas");

            migrationBuilder.DropIndex(
                name: "IX_MonHocs_NienKhoaId",
                table: "MonHocs");

            migrationBuilder.DropColumn(
                name: "NienKhoaId",
                table: "MonHocs");
        }
    }
}
