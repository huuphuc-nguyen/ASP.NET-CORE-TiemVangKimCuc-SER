using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTiemVangKimCuc.SER.Migrations
{
    /// <inheritdoc />
    public partial class addTableUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KIMCUC_USER",
                columns: table => new
                {
                    TaiKhoan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KIMCUC_USER", x => x.TaiKhoan);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KIMCUC_USER");
        }
    }
}
