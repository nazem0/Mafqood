using Domain.DTOs.ReportDTOs;
using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAsync();
        Task<IEnumerable<Report>> GetAllAsync();
        Task<Report?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(ReportAdditionDTO report);
        Task<bool> SoftDeleteAsync(Guid id, string deletionCode);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        // Add other methods as needed
    }

}
