using Funtrip.Models;

namespace Repositories.Interfaces
{
    public interface IUsuarioGrupoRepository : IRepository<UsuarioGrupo>
    {    
        UsuarioGrupo GetWithRelatedEntities(int codUsuario, int codGrupo);
    }
}