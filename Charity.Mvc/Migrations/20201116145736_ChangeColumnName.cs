using Microsoft.EntityFrameworkCore.Migrations;

namespace Charity.Mvc.Migrations
{
    public partial class ChangeColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desctiption",
                table: "Institutions");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Institutions",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "c50e11f1-fdb7-46c0-85b6-b2191a994aa3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "5ed21a94-fcfd-4aff-9c73-a98ff0f48c47");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "bbce6ebe-70ee-449c-b0ea-eeec9d32428f");

            migrationBuilder.UpdateData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Cel i misja: Pomoc dla osób nie posiadających miejsca zamieszkania");

            migrationBuilder.UpdateData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Cel i misja: Pomoc osobom znajdującym się w trudnej sytuacji życiowej");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Institutions");

            migrationBuilder.AddColumn<string>(
                name: "Desctiption",
                table: "Institutions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "5d186f66-2bdf-481c-9a29-c757a0658fab");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "f8c8bea0-df56-4db9-af63-381efeebd185");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "3b706ec3-59a3-4e50-963d-a8dd6bc4f1f4");

            migrationBuilder.UpdateData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Desctiption",
                value: "Cel i misja: Pomoc dla osób nie posiadających miejsca zamieszkania");

            migrationBuilder.UpdateData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Desctiption",
                value: "Cel i misja: Pomoc osobom znajdującym się w trudnej sytuacji życiowej");
        }
    }
}
