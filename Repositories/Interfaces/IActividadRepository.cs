using Travlr.Models;

namespace Repositories.Interfaces
{
    public interface IActividadRepository : IRepository<Actividad>
    {
        Actividad GetPeroCompleto(int id);
    }
}