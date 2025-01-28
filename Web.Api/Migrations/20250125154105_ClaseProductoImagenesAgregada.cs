using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class ClaseProductoImagenesAgregada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlFotos",
                table: "Productos");

            migrationBuilder.CreateTable(
                name: "ProductoImagenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    UrlImagen = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoImagenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductoImagenes_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductoImagenes_ProductoId",
                table: "ProductoImagenes",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductoImagenes");

            migrationBuilder.AddColumn<string>(
                name: "UrlFotos",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
