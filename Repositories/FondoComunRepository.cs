using Funtrip.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class FondoComunRepository : Repository<FondoComun>, IFondoComunRepository
    {
        public FondoComunRepository(DbContext context) : base(context)
        {
        }

        public DbContext DbContext{ get { return Context as DbContext; } }
    }
}