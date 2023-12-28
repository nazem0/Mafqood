using System.Net;

namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        HttpStatusCode SaveChanges();
        Task<HttpStatusCode> SaveChangesAsync();
    }
}
