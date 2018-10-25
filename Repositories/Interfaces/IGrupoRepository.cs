using Travlr.Models;

namespace Repositories.Interfaces
{
    public interface IGrupoRepository : IRepository<Grupo>
    {
        Grupo GetWithRelatedEntities(int id);    
    }
}