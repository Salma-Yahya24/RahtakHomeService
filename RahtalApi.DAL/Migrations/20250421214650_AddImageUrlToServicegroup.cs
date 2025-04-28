using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RahtakApi.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToServicegroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ServiceGroups",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ServiceGroups");
        }
    }
}
