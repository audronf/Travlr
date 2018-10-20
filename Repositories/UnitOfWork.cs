using Funtrip.Repositories.Database;
using Repositories;
using Repositories.Interfaces;

 namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext _context;
        public UnitOfWork(DataBaseContext context)
        {
            _context = context;
            ActividadRepository = new ActividadRepository(_context);
            EncuestaRepository = new EncuestaRepository(_context);
            FechaDisponibilidadRepository = new FechaDisponibilidadRepository(_context);
            FondoComunRepository = new FondoComunRepository(_context);
            GrupoRepository = new GrupoRepository(_context);
            UsuarioRepository = new UsuarioRepository(_context);
        }
        public IActividadRepository ActividadRepository { get; private set; }
        public IEncuestaRepository EncuestaRepository { get; private set; }
        public IFechaDisponibilidadRepository FechaDisponibilidadRepository { get; private set; }
        public IFondoComunRepository FondoComunRepository { get; private set; }
        public IGrupoRepository GrupoRepository { get; private set; }
        public IUsuarioRepository UsuarioRepository { get; private set; }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}