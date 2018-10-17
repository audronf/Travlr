using Funtrip.Repositories.Database;
using Repositories.Interfaces;
 namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext _context;
         public UnitOfWork(DataBaseContext context)
        {
            _context = context;
        }
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