using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingFileStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileStorageId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileStorages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BucketName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileStorages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FileStorageId",
                table: "AspNetUsers",
                column: "FileStorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FileStorages_FileStorageId",
                table: "AspNetUsers",
                column: "FileStorageId",
                principalTable: "FileStorages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FileStorages_FileStorageId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "FileStorages");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FileStorageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FileStorageId",
                table: "AspNetUsers");
        }
    }
}
