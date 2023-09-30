using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentFlex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedEestate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Estates_EstateId",
                table: "Rentals");

            migrationBuilder.AlterColumn<string>(
                name: "EstateType",
                table: "Estates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Estates",
                columns: new[] { "Id", "Address_City", "Address_Country", "Address_PostalCode", "Address_PropertyNumber", "Address_StreetName", "AirbnbReference", "BookingReference", "CostPerDay", "EstateType", "ImageUrls", "IsAvailable", "OwnerId", "PropertyName" },
                values: new object[] { new Guid("555daf1f-c760-48d4-9fcf-410cec349f23"), "Gdańsk", "Poland", "80-342", 11, "Grunwaldzka", null, null, 200.0, "Apartment", null, true, new Guid("6aa43469-b1c8-42b1-aa67-b7240a575f0a"), "TestProperty" });

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Estates_EstateId",
                table: "Rentals",
                column: "EstateId",
                principalTable: "Estates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Estates_EstateId",
                table: "Rentals");

            migrationBuilder.DeleteData(
                table: "Estates",
                keyColumn: "Id",
                keyValue: new Guid("555daf1f-c760-48d4-9fcf-410cec349f23"));

            migrationBuilder.AlterColumn<int>(
                name: "EstateType",
                table: "Estates",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Estates_EstateId",
                table: "Rentals",
                column: "EstateId",
                principalTable: "Estates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
