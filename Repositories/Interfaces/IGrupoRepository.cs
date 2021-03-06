using Funtrip.Models;

namespace Repositories.Interfaces
{
    public interface IGrupoRepository : IRepository<Grupo>
    {    
        Grupo GetGrupoByIdGrupo(int GrupoID);
    }
}