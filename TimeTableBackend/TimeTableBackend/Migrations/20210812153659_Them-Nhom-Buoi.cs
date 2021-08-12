using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTableBackend.Migrations
{
    public partial class ThemNhomBuoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buois",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SoTiet = table.Column<int>(type: "INTEGER", nullable: false),
                    BatDauLuc = table.Column<int>(type: "INTEGER", nullable: false),
                    TietBatDau = table.Column<int>(type: "INTEGER", nullable: false),
                    TietKetThuc = table.Column<int>(type: "INTEGER", nullable: false),
                    NhomMonHocId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buois", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buois_NhomMonHocs_NhomMonHocId",
                        column: x => x.NhomMonHocId,
                        principalTable: "NhomMonHocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buois_NhomMonHocId",
                table: "Buois",
                column: "NhomMonHocId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buois");
        }
    }
}
