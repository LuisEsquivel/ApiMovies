using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiMovies.Migrations
{
    public partial class TablaPeliculaAddTipoClasificacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Clasificacion",
                table: "Pelicula",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Clasificacion",
                table: "Pelicula");
        }
    }
}
