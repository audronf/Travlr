using Funtrip.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class ActividadRepository : Repository<Actividad>, IActividadRepository
    {
        public ActividadRepository(DbContext context) : base(context)
        {
        }
    }
}