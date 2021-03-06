using Funtrip.Models;
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
        public Grupo GetGrupoByIdGrupo(int GrupoID)
        {
            var grupo = Context.Set<Grupo>().Find(GrupoID);
            return grupo;
        }
    }
}