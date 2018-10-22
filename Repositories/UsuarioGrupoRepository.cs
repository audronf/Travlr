using Funtrip.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class UsuarioGrupoRepository : Repository<UsuarioGrupo>, IUsuarioGrupoRepository
    {
        public UsuarioGrupoRepository(DbContext context) : base(context)
        {
        }
    }
}