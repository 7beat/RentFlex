using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentFlex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedThumbnailImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThumbnailImage",
                table: "Estates",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Estates",
                keyColumn: "Id",
                keyValue: new Guid("555daf1f-c760-48d4-9fcf-410cec349f23"),
                column: "ThumbnailImage",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailImage",
                table: "Estates");
        }
    }
}
