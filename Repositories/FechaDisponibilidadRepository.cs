using Funtrip.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class FechaDisponibilidadRepository : Repository<FechaDisponibilidad>, IFechaDisponibilidadRepository
    {
        public FechaDisponibilidadRepository(DbContext context) : base(context)
        {
        }
    }
}