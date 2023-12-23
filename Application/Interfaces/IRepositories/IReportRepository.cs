using Application.DTOs.ReportDTOs;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces.IRepositories
{
    public interface IReportRepository
    {
        PaginationViewDTO<ReportViewDTO> Get(int pageIndex, int pageSize, ReportFiltrationDTO filter);
        PaginationViewDTO<ReportViewDTO> GetAll(int pageIndex, int pageSize, ReportFiltrationDTO filter);
        PaginationViewDTO<ReportViewDTO> GetUnvalid(int pageIndex, int pageSize);
        Task<ReportViewDTO?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(ReportAdditionDTO report);
        Task<bool> SoftDeleteAsync(Guid id, string deletionCode);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        // Add other methods as needed
    }

}
