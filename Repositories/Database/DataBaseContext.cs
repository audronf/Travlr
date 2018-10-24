#region Using
using Microsoft.EntityFrameworkCore;
using Travlr.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
#endregion
namespace Travlr.Repositories.Database
{
    public class DataBaseContext : IdentityDbContext<Usuario>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) {}
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Encuesta> Encuestas { get; set; }
        public DbSet<FechaDisponibilidad> FechasDisponibilidad { get; set; }
        public DbSet<FondoComun> FondosComunes { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioGrupo>()
            .HasKey(g => new { g.UsuarioId, g.GrupoID });

            modelBuilder.Entity<UsuarioGrupo>()
            .HasOne(us => us.Usuario)
            .WithMany(gr => gr.UsuarioGrupos)
            .HasForeignKey(us => us.UsuarioId);

            modelBuilder.Entity<UsuarioGrupo>()
            .HasOne(us => us.Grupo)
            .WithMany(gr => gr.UsuarioGrupos)
            .HasForeignKey(us => us.GrupoID);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}