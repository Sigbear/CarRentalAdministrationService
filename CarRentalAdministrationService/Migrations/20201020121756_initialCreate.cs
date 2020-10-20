using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRentalAdministrationService.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarCategories",
                columns: table => new
                {
                    CarCategoryId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(nullable: true),
                    BaseDayRentalCost = table.Column<int>(nullable: false),
                    KilometerPrice = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarCategories", x => x.CarCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    CustomerDateOfBirth = table.Column<DateTime>(nullable: false),
                    CarCategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_CarCategories_CarCategoryId",
                        column: x => x.CarCategoryId,
                        principalTable: "CarCategories",
                        principalColumn: "CarCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CarCategories",
                columns: new[] { "CarCategoryId", "BaseDayRentalCost", "Category", "KilometerPrice" },
                values: new object[] { 1, 100, "Compact", 10 });

            migrationBuilder.InsertData(
                table: "CarCategories",
                columns: new[] { "CarCategoryId", "BaseDayRentalCost", "Category", "KilometerPrice" },
                values: new object[] { 2, 125, "Premium", 12 });

            migrationBuilder.InsertData(
                table: "CarCategories",
                columns: new[] { "CarCategoryId", "BaseDayRentalCost", "Category", "KilometerPrice" },
                values: new object[] { 3, 150, "Minivan", 15 });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CarCategoryId",
                table: "Orders",
                column: "CarCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "CarCategories");
        }
    }
}
