using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentFlex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: new Guid("92612953-9caa-46a9-aa78-cf3cbc246558"));

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "EndDate", "EstateId", "RentType", "StartDate" },
                values: new object[] { new Guid("50333b36-5623-4773-8148-bc0292edb7c7"), new DateTime(2024, 5, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("555daf1f-c760-48d4-9fcf-410cec349f23"), "ShortTerm", new DateTime(2024, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: new Guid("50333b36-5623-4773-8148-bc0292edb7c7"));

            migrationBuilder.DropColumn(
                name: "Username",
                table: "ApplicationUsers");

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "EndDate", "EstateId", "RentType", "StartDate" },
                values: new object[] { new Guid("92612953-9caa-46a9-aa78-cf3cbc246558"), new DateTime(2024, 5, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("555daf1f-c760-48d4-9fcf-410cec349f23"), "ShortTerm", new DateTime(2024, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
