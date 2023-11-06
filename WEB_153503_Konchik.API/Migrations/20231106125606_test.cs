using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_153503_Konchik.API.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tools_Categories_CategoryId",
                table: "Tools");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Tools",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tools_Categories_CategoryId",
                table: "Tools",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tools_Categories_CategoryId",
                table: "Tools");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Tools",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Tools_Categories_CategoryId",
                table: "Tools",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
