using Travlr.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Linq;

namespace Repositories
{
    public class GrupoRepository : Repository<Grupo>, IGrupoRepository
    {
        public GrupoRepository(DbContext context) : base(context)
        {
        }
        public DbContext DbContext{ get { return Context as DbContext; } }
        public Grupo GetPeroCompleto(int cod)
        {
            var grupo = Context.Set<Grupo>().Include(x => x.FondoComun).Include(x => x.Encuestas).Include(x => x.Actividades).Include(x => x.FechasDisponibilidad).Where(gr => gr.GrupoID == cod).FirstOrDefault();
            return grupo;
        }
    }
}