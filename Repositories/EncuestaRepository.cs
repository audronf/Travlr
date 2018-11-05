using Travlr.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Linq;

namespace Repositories
{
    public class EncuestaRepository : Repository<Encuesta>, IEncuestaRepository
    {
        public EncuestaRepository(DbContext context) : base(context)
        {
        }
        public Encuesta GetPeroCompleto(int cod)
        {
            var encuesta = Context.Set<Encuesta>().Include(x => x.Opciones).Include(x => x.Votaron).Where(en => en.ID == cod).FirstOrDefault();
            return encuesta;
        }
    }
}