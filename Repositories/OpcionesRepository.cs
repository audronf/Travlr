using Travlr.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class OpcionRepository : Repository<Opcion>, IOpcionRepository
    {
        public OpcionRepository(DbContext context) : base(context)
        {
        }
    }
}