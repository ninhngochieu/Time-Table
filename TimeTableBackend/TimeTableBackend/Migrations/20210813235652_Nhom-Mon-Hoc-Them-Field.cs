using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTableBackend.Migrations
{
    public partial class NhomMonHocThemField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhomMonHocs_MonHocs_MonHocId",
                table: "NhomMonHocs");

            migrationBuilder.AlterColumn<int>(
                name: "MonHocId",
                table: "NhomMonHocs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NhomMonHocs_MonHocs_MonHocId",
                table: "NhomMonHocs",
                column: "MonHocId",
                principalTable: "MonHocs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhomMonHocs_MonHocs_MonHocId",
                table: "NhomMonHocs");

            migrationBuilder.AlterColumn<int>(
                name: "MonHocId",
                table: "NhomMonHocs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_NhomMonHocs_MonHocs_MonHocId",
                table: "NhomMonHocs",
                column: "MonHocId",
                principalTable: "MonHocs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
