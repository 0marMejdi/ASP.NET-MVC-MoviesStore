using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hakuna.Migrations
{
    /// <inheritdoc />
    public partial class AddedTitleForMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Membershiptypes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Membershiptypes");
        }
    }
}
