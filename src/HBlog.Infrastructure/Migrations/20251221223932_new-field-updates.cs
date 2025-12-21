using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newfieldupdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileData_FileStorages_FileStorageId",
                table: "FileData");

            migrationBuilder.DropForeignKey(
                name: "FK_User_FileStorages_FileStorageId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileStorages",
                table: "FileStorages");

            migrationBuilder.DropColumn(
                name: "City",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Interests",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LookingFor",
                table: "User");

            migrationBuilder.RenameTable(
                name: "FileStorages",
                newName: "FileStorage");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Posts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Posts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Posts",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileStorage",
                table: "FileStorage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileData_FileStorage_FileStorageId",
                table: "FileData",
                column: "FileStorageId",
                principalTable: "FileStorage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_FileStorage_FileStorageId",
                table: "User",
                column: "FileStorageId",
                principalTable: "FileStorage",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileData_FileStorage_FileStorageId",
                table: "FileData");

            migrationBuilder.DropForeignKey(
                name: "FK_User_FileStorage_FileStorageId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileStorage",
                table: "FileStorage");

            migrationBuilder.RenameTable(
                name: "FileStorage",
                newName: "FileStorages");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Interests",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LookingFor",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Posts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Posts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Posts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileStorages",
                table: "FileStorages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileData_FileStorages_FileStorageId",
                table: "FileData",
                column: "FileStorageId",
                principalTable: "FileStorages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_FileStorages_FileStorageId",
                table: "User",
                column: "FileStorageId",
                principalTable: "FileStorages",
                principalColumn: "Id");
        }
    }
}
