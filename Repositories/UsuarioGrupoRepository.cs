using System;
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

        public UsuarioGrupo GetWithRelatedEntities(int codUsuario, int codGrupo)
        {
            var usuarioGrupo = Context.Set<UsuarioGrupo>().Find(codUsuario.ToString(), codGrupo);
            return usuarioGrupo;
        }
    }
}