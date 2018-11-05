using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Travlr.Migrations
{
    public partial class YaNoSeQuePonerleMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Actividades_ActividadID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Encuestas_EncuestaID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ActividadID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EncuestaID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ActividadID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EncuestaID",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ActividadConfirmado",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UsuarioId = table.Column<string>(nullable: true),
                    Asiste = table.Column<bool>(nullable: false),
                    ActividadID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActividadConfirmado", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ActividadConfirmado_Actividades_ActividadID",
                        column: x => x.ActividadID,
                        principalTable: "Actividades",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Votaron",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UsuarioId = table.Column<string>(nullable: true),
                    Voto = table.Column<bool>(nullable: false),
                    EncuestaID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votaron", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Votaron_Encuestas_EncuestaID",
                        column: x => x.EncuestaID,
                        principalTable: "Encuestas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActividadConfirmado_ActividadID",
                table: "ActividadConfirmado",
                column: "ActividadID");

            migrationBuilder.CreateIndex(
                name: "IX_Votaron_EncuestaID",
                table: "Votaron",
                column: "EncuestaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActividadConfirmado");

            migrationBuilder.DropTable(
                name: "Votaron");

            migrationBuilder.AddColumn<int>(
                name: "ActividadID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EncuestaID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ActividadID",
                table: "AspNetUsers",
                column: "ActividadID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EncuestaID",
                table: "AspNetUsers",
                column: "EncuestaID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Actividades_ActividadID",
                table: "AspNetUsers",
                column: "ActividadID",
                principalTable: "Actividades",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Encuestas_EncuestaID",
                table: "AspNetUsers",
                column: "EncuestaID",
                principalTable: "Encuestas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
