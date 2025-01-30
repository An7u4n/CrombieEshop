using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUrlToS3KeyUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagen_FotoPerfilUrl",
                table: "Usuarios");

            migrationBuilder.AddColumn<string>(
                name: "Imagen_FotoPerfilKey",
                table: "Usuarios",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagen_FotoPerfilKey",
                table: "Usuarios");

            migrationBuilder.AddColumn<string>(
                name: "Imagen_FotoPerfilUrl",
                table: "Usuarios",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);
        }
    }
}
