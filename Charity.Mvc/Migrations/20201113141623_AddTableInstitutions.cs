using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Charity.Mvc.Migrations
{
    public partial class AddTableInstitutions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DonationId",
                table: "Institutions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DonationId",
                table: "Categories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(nullable: false),
                    Street = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    PickUpOn = table.Column<DateTime>(nullable: false),
                    PickUpComment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Donations_DonationId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Institutions_Donations_DonationId",
                table: "Institutions");

            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Institutions_DonationId",
                table: "Institutions");

            migrationBuilder.DropIndex(
                name: "IX_Categories_DonationId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DonationId",
                table: "Institutions");

            migrationBuilder.DropColumn(
                name: "DonationId",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "719c1394-afc5-44a1-99ae-b2a5e4fdb3e6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "de7eb052-48e1-4277-af48-1008af6f1278");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "85fc5145-daa6-40ad-bd49-b14cf9a09607");
        }
    }
}
