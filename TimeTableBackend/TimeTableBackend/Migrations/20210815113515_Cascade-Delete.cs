using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTableBackend.Migrations
{
    public partial class CascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buois_NhomMonHocs_NhomMonHocId",
                table: "Buois");

            migrationBuilder.DropForeignKey(
                name: "FK_MonHocs_NienKhoas_NienKhoaId",
                table: "MonHocs");

            migrationBuilder.AlterColumn<int>(
                name: "NienKhoaId",
                table: "MonHocs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NhomMonHocId",
                table: "Buois",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Buois_NhomMonHocs_NhomMonHocId",
                table: "Buois",
                column: "NhomMonHocId",
                principalTable: "NhomMonHocs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonHocs_NienKhoas_NienKhoaId",
                table: "MonHocs",
                column: "NienKhoaId",
                principalTable: "NienKhoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buois_NhomMonHocs_NhomMonHocId",
                table: "Buois");

            migrationBuilder.DropForeignKey(
                name: "FK_MonHocs_NienKhoas_NienKhoaId",
                table: "MonHocs");

            migrationBuilder.AlterColumn<int>(
                name: "NienKhoaId",
                table: "MonHocs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "NhomMonHocId",
                table: "Buois",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Buois_NhomMonHocs_NhomMonHocId",
                table: "Buois",
                column: "NhomMonHocId",
                principalTable: "NhomMonHocs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MonHocs_NienKhoas_NienKhoaId",
                table: "MonHocs",
                column: "NienKhoaId",
                principalTable: "NienKhoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
