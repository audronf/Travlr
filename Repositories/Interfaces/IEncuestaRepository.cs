using Travlr.Models;

namespace Repositories.Interfaces
{
    public interface IEncuestaRepository : IRepository<Encuesta>
    {
        Encuesta GetPeroCompleto(int cod);    
    }
}