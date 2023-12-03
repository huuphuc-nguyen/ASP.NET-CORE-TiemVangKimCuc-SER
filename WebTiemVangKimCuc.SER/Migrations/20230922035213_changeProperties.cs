using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTiemVangKimCuc.SER.Migrations
{
    /// <inheritdoc />
    public partial class changeProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TrongLuongSanPham",
                table: "SAN_PHAM",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "TrongLuongSanPham",
                table: "SAN_PHAM",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
