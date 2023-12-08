using Domain.DTOs.ReportDTOs;
using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IReportRepository
    {
        IEnumerable<Report> Get(ReportFiltrationDTO? filter = null);
        IEnumerable<Report> GetAll();
        Task<Report?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(ReportAdditionDTO report);
        Task<bool> SoftDeleteAsync(Guid id, string deletionCode);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        // Add other methods as needed
    }

}
