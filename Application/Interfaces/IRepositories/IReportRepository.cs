using Application.DTOs.ReportDTOs;
using Domain.DTOs;
using Domain.Entities;
using System.Net;

namespace Application.Interfaces.IRepositories
{
    public interface IReportRepository
    {
        PaginationViewDTO<ReportViewDTO> Get(int pageIndex, int pageSize, ReportFiltrationDTO filter);
        PaginationViewDTO<ReportViewDTO> GetAll(int pageIndex, int pageSize, ReportFiltrationDTO filter);
        Task<ReportViewDTO?> GetByIdAsync(Guid id);
        Task<HttpStatusCode> AddAsync(ReportAdditionDTO report);
        Task<HttpStatusCode> SoftDeleteAsync(Guid id, string deletionCode);
        Task<HttpStatusCode> DeleteAsync(Guid id);
        Task<HttpStatusCode> ApproveAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        // Add other methods as needed
    }

}
