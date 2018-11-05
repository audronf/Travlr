using Microsoft.EntityFrameworkCore.Migrations;

namespace Travlr.Migrations
{
    public partial class RelationFixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opcion_Encuestas_EncuestaID",
                table: "Opcion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Opcion",
                table: "Opcion");

            migrationBuilder.RenameTable(
                name: "Opcion",
                newName: "Opciones");

            migrationBuilder.RenameIndex(
                name: "IX_Opcion_EncuestaID",
                table: "Opciones",
                newName: "IX_Opciones_EncuestaID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Opciones",
                table: "Opciones",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Opciones_Encuestas_EncuestaID",
                table: "Opciones",
                column: "EncuestaID",
                principalTable: "Encuestas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opciones_Encuestas_EncuestaID",
                table: "Opciones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Opciones",
                table: "Opciones");

            migrationBuilder.RenameTable(
                name: "Opciones",
                newName: "Opcion");

            migrationBuilder.RenameIndex(
                name: "IX_Opciones_EncuestaID",
                table: "Opcion",
                newName: "IX_Opcion_EncuestaID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Opcion",
                table: "Opcion",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Opcion_Encuestas_EncuestaID",
                table: "Opcion",
                column: "EncuestaID",
                principalTable: "Encuestas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
