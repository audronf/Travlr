using System;
namespace Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {   
        IActividadRepository ActividadRepository { get; }
        IEncuestaRepository EncuestaRepository { get; }
        IFechaDisponibilidadRepository FechaDisponibilidadRepository { get; }
        IFondoComunRepository FondoComunRepository { get; }
        IGrupoRepository GrupoRepository { get; }
        IUsuarioGrupoRepository UsuarioGrupoRepository { get; }
        IOpcionRepository OpcionRepository { get; }
        int Complete();
    }
}