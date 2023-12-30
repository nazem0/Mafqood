using Application.DTOs.ReportDTOs;
using Domain.DTOs;
using Domain.Entities;
using System.Net;

namespace Application.Interfaces.IRepositories
{
    public interface IReportRepository
    {
        Task<PaginationViewDTO<ReportViewDTO>> GetAsync(int pageIndex, int pageSize, ReportFiltrationDTO filter);
        Task<PaginationViewDTO<ReportViewDTO>> GetAllAsync(int pageIndex, int pageSize, ReportFiltrationDTO filter);
        Task<ReportViewDTO?> GetByIdAsync(Guid id);
        Task<HttpStatusCode> AddAsync(ReportAdditionDTO report);
        Task<HttpStatusCode> SoftDeleteAsync(Guid id, string deletionCode);
        Task<HttpStatusCode> DeleteAsync(Guid id);
        Task<HttpStatusCode> ApproveAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        // Add other methods as needed
    }

}
