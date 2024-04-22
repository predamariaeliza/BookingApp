using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeededPropertiesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "CreatedDate", "Description", "ImageUrl", "Name", "Occupancy", "Price", "SquareMeters", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, "Description for Premium Pool Villa", "https://placehold.co/600x401", "Premium Pool Villa", 3.0, 1000.0, 100.0, null },
                    { 2, null, "Description for Austrian Villa", "https://placehold.co/600x402", "Austrian Villa", 2.0, 2000.0, 200.0, null },
                    { 3, null, "Description for Italian Villa", "https://placehold.co/600x403", "Italian Villa", 1.0, 3000.0, 300.0, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
