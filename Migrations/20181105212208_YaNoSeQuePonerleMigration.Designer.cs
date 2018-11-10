﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Travlr.Repositories.Database;

namespace Travlr.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20181105212208_YaNoSeQuePonerleMigration")]
    partial class YaNoSeQuePonerleMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Travlr.Models.Actividad", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descripcion");

                    b.Property<DateTime>("FechaHora");

                    b.Property<int?>("GrupoID");

                    b.HasKey("ID");

                    b.HasIndex("GrupoID");

                    b.ToTable("Actividades");
                });

            modelBuilder.Entity("Travlr.Models.ActividadConfirmado", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ActividadID");

                    b.Property<bool>("Asiste");

                    b.Property<string>("UsuarioId");

                    b.HasKey("ID");

                    b.HasIndex("ActividadID");

                    b.ToTable("ActividadConfirmado");
                });

            modelBuilder.Entity("Travlr.Models.Encuesta", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GrupoID");

                    b.Property<string>("Pregunta");

                    b.HasKey("ID");

                    b.HasIndex("GrupoID");

                    b.ToTable("Encuestas");
                });

            modelBuilder.Entity("Travlr.Models.FechaDisponibilidad", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("FechaFin");

                    b.Property<DateTime>("FechaInicio");

                    b.Property<int?>("GrupoID");

                    b.Property<string>("UsuarioId");

                    b.HasKey("ID");

                    b.HasIndex("GrupoID");

                    b.ToTable("FechasDisponibilidad");
                });

            modelBuilder.Entity("Travlr.Models.FondoComun", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Monto");

                    b.HasKey("ID");

                    b.ToTable("FondosComunes");
                });

            modelBuilder.Entity("Travlr.Models.Grupo", b =>
                {
                    b.Property<int>("GrupoID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdministradorId");

                    b.Property<int?>("FondoComunID");

                    b.Property<string>("Nombre");

                    b.HasKey("GrupoID");

                    b.HasIndex("AdministradorId");

                    b.HasIndex("FondoComunID");

                    b.ToTable("Grupos");
                });

            modelBuilder.Entity("Travlr.Models.Opcion", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Cantidad");

                    b.Property<int?>("EncuestaID");

                    b.Property<string>("Texto");

                    b.HasKey("ID");

                    b.HasIndex("EncuestaID");

                    b.ToTable("Opciones");
                });

            modelBuilder.Entity("Travlr.Models.Usuario", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NickName");

                    b.Property<string>("Nombre");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("Password");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Travlr.Models.UsuarioGrupo", b =>
                {
                    b.Property<string>("UsuarioId");

                    b.Property<int>("GrupoID");

                    b.HasKey("UsuarioId", "GrupoID");

                    b.HasIndex("GrupoID");

                    b.ToTable("UsuarioGrupo");
                });

            modelBuilder.Entity("Travlr.Models.Votaron", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("EncuestaID");

                    b.Property<string>("UsuarioId");

                    b.Property<bool>("Voto");

                    b.HasKey("ID");

                    b.HasIndex("EncuestaID");

                    b.ToTable("Votaron");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Travlr.Models.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Travlr.Models.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Travlr.Models.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Travlr.Models.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Travlr.Models.Actividad", b =>
                {
                    b.HasOne("Travlr.Models.Grupo")
                        .WithMany("Actividades")
                        .HasForeignKey("GrupoID");
                });

            modelBuilder.Entity("Travlr.Models.ActividadConfirmado", b =>
                {
                    b.HasOne("Travlr.Models.Actividad")
                        .WithMany("Confirmados")
                        .HasForeignKey("ActividadID");
                });

            modelBuilder.Entity("Travlr.Models.Encuesta", b =>
                {
                    b.HasOne("Travlr.Models.Grupo")
                        .WithMany("Encuestas")
                        .HasForeignKey("GrupoID");
                });

            modelBuilder.Entity("Travlr.Models.FechaDisponibilidad", b =>
                {
                    b.HasOne("Travlr.Models.Grupo")
                        .WithMany("FechasDisponibilidad")
                        .HasForeignKey("GrupoID");
                });

            modelBuilder.Entity("Travlr.Models.Grupo", b =>
                {
                    b.HasOne("Travlr.Models.Usuario", "Administrador")
                        .WithMany("GruposAdmin")
                        .HasForeignKey("AdministradorId");

                    b.HasOne("Travlr.Models.FondoComun", "FondoComun")
                        .WithMany()
                        .HasForeignKey("FondoComunID");
                });

            modelBuilder.Entity("Travlr.Models.Opcion", b =>
                {
                    b.HasOne("Travlr.Models.Encuesta")
                        .WithMany("Opciones")
                        .HasForeignKey("EncuestaID");
                });

            modelBuilder.Entity("Travlr.Models.UsuarioGrupo", b =>
                {
                    b.HasOne("Travlr.Models.Grupo", "Grupo")
                        .WithMany("UsuarioGrupos")
                        .HasForeignKey("GrupoID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Travlr.Models.Usuario", "Usuario")
                        .WithMany("UsuarioGrupos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Travlr.Models.Votaron", b =>
                {
                    b.HasOne("Travlr.Models.Encuesta")
                        .WithMany("Votaron")
                        .HasForeignKey("EncuestaID");
                });
#pragma warning restore 612, 618
        }
    }
}