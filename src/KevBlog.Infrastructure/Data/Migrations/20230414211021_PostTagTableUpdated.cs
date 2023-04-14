using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class PostTagTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PostTags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTags", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PostTags_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_TagId",
                table: "PostTags",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostTags");

            migrationBuilder.DropColumn(
                name: "Desc",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Posts");
        }
    }
}
