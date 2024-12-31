using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class RelacionUsuarioWishListCambiada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WishLists_UsuarioId",
                table: "WishLists");

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_UsuarioId",
                table: "WishLists",
                column: "UsuarioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WishLists_UsuarioId",
                table: "WishLists");

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_UsuarioId",
                table: "WishLists",
                column: "UsuarioId");
        }
    }
}
