using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTiemVangKimCuc.SER.Migrations
{
    /// <inheritdoc />
    public partial class addTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DM_CHAT_LIEU",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatLieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DM_CHAT_LIEU", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DM_TRANG_SUC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoaiTrangSuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DM_TRANG_SUC", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SAN_PHAM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenSanPham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrongLuongSanPham = table.Column<float>(type: "real", nullable: false),
                    ChatLieu_ID = table.Column<int>(type: "int", nullable: false),
                    LoaiTrangSuc_ID = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", unicode: false, fixedLength: true, maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAN_PHAM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAN_PHAM_DM_CHAT_LIEU_ChatLieu_ID",
                        column: x => x.ChatLieu_ID,
                        principalTable: "DM_CHAT_LIEU",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SAN_PHAM_DM_TRANG_SUC_LoaiTrangSuc_ID",
                        column: x => x.LoaiTrangSuc_ID,
                        principalTable: "DM_TRANG_SUC",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SAN_PHAM_ChatLieu_ID",
                table: "SAN_PHAM",
                column: "ChatLieu_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SAN_PHAM_LoaiTrangSuc_ID",
                table: "SAN_PHAM",
                column: "LoaiTrangSuc_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SAN_PHAM");

            migrationBuilder.DropTable(
                name: "DM_CHAT_LIEU");

            migrationBuilder.DropTable(
                name: "DM_TRANG_SUC");
        }
    }
}
