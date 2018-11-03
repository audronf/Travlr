using Microsoft.EntityFrameworkCore.Migrations;

namespace Travlr.Migrations
{
    public partial class ReforgeAttributeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FechasDisponibilidad_AspNetUsers_UsuarioId",
                table: "FechasDisponibilidad");

            migrationBuilder.DropIndex(
                name: "IX_FechasDisponibilidad_UsuarioId",
                table: "FechasDisponibilidad");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FechasDisponibilidad_UsuarioId",
                table: "FechasDisponibilidad",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_FechasDisponibilidad_AspNetUsers_UsuarioId",
                table: "FechasDisponibilidad",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
