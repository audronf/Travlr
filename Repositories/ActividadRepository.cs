using Travlr.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Linq;

namespace Repositories
{
    public class ActividadRepository : Repository<Actividad>, IActividadRepository
    {
        public ActividadRepository(DbContext context) : base(context)
        {
        }

        public Actividad GetPeroCompleto(int id)
        {
            var actividad = Context.Set<Actividad>().Include(x => x.Confirmados).Where(ac => ac.ID == id).FirstOrDefault();
            return actividad;
        }
    }
}