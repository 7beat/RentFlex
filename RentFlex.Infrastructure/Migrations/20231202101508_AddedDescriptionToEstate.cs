using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentFlex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedDescriptionToEstate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: new Guid("c36678ef-4941-42cc-913f-03e3b0608238"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Estates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Estates",
                keyColumn: "Id",
                keyValue: new Guid("555daf1f-c760-48d4-9fcf-410cec349f23"),
                columns: new[] { "Description", "PropertyName" },
                values: new object[] { "Serene, Elegant, Nature-Inspired Haven. Perfect for couples and families.", "Cosy Retreat" });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "EndDate", "EstateId", "RentType", "StartDate" },
                values: new object[] { new Guid("77912577-fa49-4ff9-85ff-b8c8dda3855f"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("555daf1f-c760-48d4-9fcf-410cec349f23"), "ShortTerm", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: new Guid("77912577-fa49-4ff9-85ff-b8c8dda3855f"));

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Estates");

            migrationBuilder.UpdateData(
                table: "Estates",
                keyColumn: "Id",
                keyValue: new Guid("555daf1f-c760-48d4-9fcf-410cec349f23"),
                column: "PropertyName",
                value: "TestProperty");

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "EndDate", "EstateId", "RentType", "StartDate" },
                values: new object[] { new Guid("c36678ef-4941-42cc-913f-03e3b0608238"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("555daf1f-c760-48d4-9fcf-410cec349f23"), "ShortTerm", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
