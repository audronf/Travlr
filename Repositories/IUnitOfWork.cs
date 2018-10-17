using System;
 namespace Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {   
        int Complete();
    }
}