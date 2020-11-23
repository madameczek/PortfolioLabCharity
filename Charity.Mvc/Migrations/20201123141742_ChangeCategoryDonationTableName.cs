using Microsoft.EntityFrameworkCore.Migrations;

namespace Charity.Mvc.Migrations
{
    public partial class ChangeCategoryDonationTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDonationModel_Categories_CategoryId",
                table: "CategoryDonationModel");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDonationModel_Donations_DonationId",
                table: "CategoryDonationModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryDonationModel",
                table: "CategoryDonationModel");

            migrationBuilder.RenameTable(
                name: "CategoryDonationModel",
                newName: "CategoryDonation");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryDonationModel_DonationId",
                table: "CategoryDonation",
                newName: "IX_CategoryDonation_DonationId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryDonationModel_CategoryId",
                table: "CategoryDonation",
                newName: "IX_CategoryDonation_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryDonation",
                table: "CategoryDonation",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "50081b0a-e847-4345-961f-a97cb9edaca3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "b50581e3-90e4-4e2b-9fd4-c40d486289cb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "a9e66cd8-98de-4f0d-854c-1881cbf68cd0");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDonation_Categories_CategoryId",
                table: "CategoryDonation",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDonation_Donations_DonationId",
                table: "CategoryDonation",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDonation_Categories_CategoryId",
                table: "CategoryDonation");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDonation_Donations_DonationId",
                table: "CategoryDonation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryDonation",
                table: "CategoryDonation");

            migrationBuilder.RenameTable(
                name: "CategoryDonation",
                newName: "CategoryDonationModel");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryDonation_DonationId",
                table: "CategoryDonationModel",
                newName: "IX_CategoryDonationModel_DonationId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryDonation_CategoryId",
                table: "CategoryDonationModel",
                newName: "IX_CategoryDonationModel_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryDonationModel",
                table: "CategoryDonationModel",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "2ebdd6f4-0e68-454c-99ed-ec8c86b1a447");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "b581f770-991b-4cd2-87d4-74ea20de1684");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "79834643-c964-4bb1-a4f0-1f41b3373bbe");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDonationModel_Categories_CategoryId",
                table: "CategoryDonationModel",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDonationModel_Donations_DonationId",
                table: "CategoryDonationModel",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
