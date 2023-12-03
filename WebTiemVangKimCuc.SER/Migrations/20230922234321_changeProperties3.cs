using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTiemVangKimCuc.SER.Migrations
{
    /// <inheritdoc />
    public partial class changeProperties3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "SAN_PHAM",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "SAN_PHAM");
        }
    }
}
