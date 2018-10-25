using Travlr.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class GrupoRepository : Repository<Grupo>, IGrupoRepository
    {
        public GrupoRepository(DbContext context) : base(context)
        {
        }
        public DbContext DbContext{ get { return Context as DbContext; } }
        public Grupo GetWithRelatedEntities(int cod)
        {
            var culito = Context.Set<Grupo>().Find(cod);
            culito.Administrador = Context.Set<Usuario>().Find(culito.AdministradorId);
            return culito;
        }
    }
}