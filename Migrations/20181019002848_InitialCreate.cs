using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Funtrip.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "FondosComunes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Monto = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FondosComunes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Nombre = table.Column<string>(nullable: true),
                    Pass = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ActividadID = table.Column<int>(nullable: true),
                    EncuestaID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioID);
                });

            migrationBuilder.CreateTable(
                name: "Grupos",
                columns: table => new
                {
                    GrupoID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FondoComunID = table.Column<int>(nullable: true),
                    AdministradorUsuarioID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupos", x => x.GrupoID);
                    table.ForeignKey(
                        name: "FK_Grupos_Usuarios_AdministradorUsuarioID",
                        column: x => x.AdministradorUsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grupos_FondosComunes_FondoComunID",
                        column: x => x.FondoComunID,
                        principalTable: "FondosComunes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Actividades",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Descripcion = table.Column<string>(nullable: true),
                    FechaHora = table.Column<DateTime>(nullable: false),
                    GrupoID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actividades", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Actividades_Grupos_GrupoID",
                        column: x => x.GrupoID,
                        principalTable: "Grupos",
                        principalColumn: "GrupoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Encuestas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Pregunta = table.Column<string>(nullable: true),
                    Opciones = table.Column<string>(nullable: true),
                    GrupoID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encuestas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Encuestas_Grupos_GrupoID",
                        column: x => x.GrupoID,
                        principalTable: "Grupos",
                        principalColumn: "GrupoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FechasDisponibilidad",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UsuarioID = table.Column<int>(nullable: true),
                    FechaInicio = table.Column<DateTime>(nullable: false),
                    FechaFin = table.Column<DateTime>(nullable: false),
                    GrupoID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FechasDisponibilidad", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FechasDisponibilidad_Grupos_GrupoID",
                        column: x => x.GrupoID,
                        principalTable: "Grupos",
                        principalColumn: "GrupoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FechasDisponibilidad_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioGrupo",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(nullable: false),
                    GrupoID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioGrupo", x => new { x.UsuarioID, x.GrupoID });
                    table.ForeignKey(
                        name: "FK_UsuarioGrupo_Grupos_GrupoID",
                        column: x => x.GrupoID,
                        principalTable: "Grupos",
                        principalColumn: "GrupoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioGrupo_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actividades_GrupoID",
                table: "Actividades",
                column: "GrupoID");

            migrationBuilder.CreateIndex(
                name: "IX_Encuestas_GrupoID",
                table: "Encuestas",
                column: "GrupoID");

            migrationBuilder.CreateIndex(
                name: "IX_FechasDisponibilidad_GrupoID",
                table: "FechasDisponibilidad",
                column: "GrupoID");

            migrationBuilder.CreateIndex(
                name: "IX_FechasDisponibilidad_UsuarioID",
                table: "FechasDisponibilidad",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_AdministradorUsuarioID",
                table: "Grupos",
                column: "AdministradorUsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_FondoComunID",
                table: "Grupos",
                column: "FondoComunID");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioGrupo_GrupoID",
                table: "UsuarioGrupo",
                column: "GrupoID");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ActividadID",
                table: "Usuarios",
                column: "ActividadID");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EncuestaID",
                table: "Usuarios",
                column: "EncuestaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Actividades_ActividadID",
                table: "Usuarios",
                column: "ActividadID",
                principalTable: "Actividades",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Encuestas_EncuestaID",
                table: "Usuarios",
                column: "EncuestaID",
                principalTable: "Encuestas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actividades_Grupos_GrupoID",
                table: "Actividades");

            migrationBuilder.DropForeignKey(
                name: "FK_Encuestas_Grupos_GrupoID",
                table: "Encuestas");

            migrationBuilder.DropTable(
                name: "FechasDisponibilidad");

            migrationBuilder.DropTable(
                name: "UsuarioGrupo");

            migrationBuilder.DropTable(
                name: "Grupos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "FondosComunes");

            migrationBuilder.DropTable(
                name: "Actividades");

            migrationBuilder.DropTable(
                name: "Encuestas");

        }
    }
}
