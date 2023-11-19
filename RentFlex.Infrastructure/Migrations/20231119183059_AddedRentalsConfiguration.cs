using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentFlex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedRentalsConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RentType",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "EndDate", "EstateId", "RentType", "StartDate" },
                values: new object[] { new Guid("c36678ef-4941-42cc-913f-03e3b0608238"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("555daf1f-c760-48d4-9fcf-410cec349f23"), "ShortTerm", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: new Guid("c36678ef-4941-42cc-913f-03e3b0608238"));

            migrationBuilder.AlterColumn<int>(
                name: "RentType",
                table: "Rentals",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
