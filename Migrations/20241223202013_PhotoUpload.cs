using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeManager.Migrations
{
    /// <inheritdoc />
    public partial class PhotoUpload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FavoriteRecipe_MemberID",
                table: "FavoriteRecipe");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Member",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Member_UserID",
                table: "Member",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteRecipe_MemberID_RecipeID",
                table: "FavoriteRecipe",
                columns: new[] { "MemberID", "RecipeID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Member_UserID",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteRecipe_MemberID_RecipeID",
                table: "FavoriteRecipe");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Recipe");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Member",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteRecipe_MemberID",
                table: "FavoriteRecipe",
                column: "MemberID");
        }
    }
}
