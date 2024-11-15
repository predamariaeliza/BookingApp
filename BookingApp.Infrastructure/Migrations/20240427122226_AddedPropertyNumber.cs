using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertyNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Properties",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "PropertyNumbers",
                columns: table => new
                {
                    PropertyNr = table.Column<int>(type: "int", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    SpecialDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyNumbers", x => x.PropertyNr);
                    table.ForeignKey(
                        name: "FK_PropertyNumbers_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PropertyNumbers",
                columns: new[] { "PropertyNr", "PropertyId", "SpecialDetails", "Type" },
                values: new object[,]
                {
                    { 101, 1, null, 0 },
                    { 102, 1, null, 0 },
                    { 103, 1, null, 0 },
                    { 201, 2, null, 0 },
                    { 202, 2, null, 0 },
                    { 203, 2, null, 0 },
                    { 301, 3, null, 0 },
                    { 302, 3, null, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyNumbers_PropertyId",
                table: "PropertyNumbers",
                column: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyNumbers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
