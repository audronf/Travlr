using Microsoft.EntityFrameworkCore.Migrations;

namespace Travlr.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioGrupo_AspNetUsers_Id",
                table: "UsuarioGrupo");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UsuarioGrupo",
                newName: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioGrupo_AspNetUsers_UsuarioId",
                table: "UsuarioGrupo",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioGrupo_AspNetUsers_UsuarioId",
                table: "UsuarioGrupo");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "UsuarioGrupo",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioGrupo_AspNetUsers_Id",
                table: "UsuarioGrupo",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
