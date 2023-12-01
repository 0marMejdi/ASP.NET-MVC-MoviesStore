using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hakuna.Migrations
{
    /// <inheritdoc />
    public partial class AddedGUIDInCustomerForMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Membershiptypes_MembershiptypeId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "MembershiptypeId",
                table: "Customers",
                newName: "MembershipId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_MembershiptypeId",
                table: "Customers",
                newName: "IX_Customers_MembershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Membershiptypes_MembershipId",
                table: "Customers",
                column: "MembershipId",
                principalTable: "Membershiptypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Membershiptypes_MembershipId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "MembershipId",
                table: "Customers",
                newName: "MembershiptypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_MembershipId",
                table: "Customers",
                newName: "IX_Customers_MembershiptypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Membershiptypes_MembershiptypeId",
                table: "Customers",
                column: "MembershiptypeId",
                principalTable: "Membershiptypes",
                principalColumn: "Id");
        }
    }
}
