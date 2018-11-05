using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Travlr.Migrations
{
    public partial class OpcionesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Opciones",
                table: "Encuestas");

            migrationBuilder.CreateTable(
                name: "Opcion",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Texto = table.Column<string>(nullable: true),
                    Cantidad = table.Column<int>(nullable: false),
                    EncuestaID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opcion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Opcion_Encuestas_EncuestaID",
                        column: x => x.EncuestaID,
                        principalTable: "Encuestas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Opcion_EncuestaID",
                table: "Opcion",
                column: "EncuestaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Opcion");

            migrationBuilder.AddColumn<string>(
                name: "Opciones",
                table: "Encuestas",
                nullable: true);
        }
    }
}
