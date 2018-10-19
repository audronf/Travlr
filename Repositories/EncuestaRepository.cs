using Funtrip.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class EncuestaRepository : Repository<Encuesta>, IEncuestaRepository
    {
        public EncuestaRepository(DbContext context) : base(context)
        {
        }

        public DbContext DbContext{ get { return Context as DbContext; } }
    }
}