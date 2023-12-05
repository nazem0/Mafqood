using Domain.DTOs.ReportDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAsync();
        Task<Report?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(ReportAdditionDTO report);
        Task<bool> DeleteAsync(Guid id, string deletionCode);
        Task<bool> ExistsAsync(Guid id);
        // Add other methods as needed
    }

}
