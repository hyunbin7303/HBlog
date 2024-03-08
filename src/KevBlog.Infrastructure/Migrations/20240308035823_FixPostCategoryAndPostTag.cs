using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KevBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPostCategoryAndPostTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PostCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PostTags",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PostCategories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
