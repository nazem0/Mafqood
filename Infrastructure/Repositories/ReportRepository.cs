using AutoMapper;
using Domain;
using Domain.DTOs.ReportDTOs;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly EntitiesContext _context;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReportRepository> _logger;
        private DbSet<Report> _reports;

        public ReportRepository
            (EntitiesContext context, IMapper mapper, IUnitOfWork unitOfWork, ILogger<ReportRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _reports = context.Set<Report>();
        }

        public async Task<IEnumerable<Report>> GetAsync()
        {
            return await _reports.ToListAsync();
        }

        public async Task<Report?> GetByIdAsync(Guid id)
        {
            Report? report = await _reports.FindAsync(id);
            return report;
        }

        public async Task<bool> AddAsync(ReportAdditionDTO report)
        {
            Report createdReport = _mapper.Map<Report>(report);
            #region Directory Existence Checking
            string directoryPath = Path.Combine
                (Directory.GetCurrentDirectory(), "wwwroot", "Report", createdReport.Id.ToString());
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            #endregion
            await _reports.AddAsync(createdReport);
            foreach (IFormFile file in report.ReportAttachments)
            {
                FileInfo fi = new(file.FileName);
                string fileName = DateTime.Now.Ticks + fi.Extension;
                string filePath = Path.Combine(directoryPath, fileName);
                FileStream fileStream = File.Create(filePath);
                await file.CopyToAsync(fileStream);
                createdReport.Attachments.Add(new Attachment
                {
                    Name = fileName,
                    ReportId = createdReport.Id
                });
            }
            bool additionResult = Convert.ToBoolean(await _unitOfWork.SaveChangesAsync());
            return additionResult;
        }

        public async Task<bool> SoftDeleteAsync(Guid id, string deletionCode)
        {
            var report = await _reports.FindAsync(id) ?? throw new KeyNotFoundException();
            if (deletionCode != report.DeletionCode)
                throw new UnauthorizedAccessException("كود الحذف الذي قمت بإدخاله غير صحيح");
            report.Missing = false;
            _reports.Update(report);
            bool deletionResult = Convert.ToBoolean(await _unitOfWork.SaveChangesAsync());
            return deletionResult;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var report = await _reports.FindAsync(id) ?? throw new KeyNotFoundException();
            string directoryPath = Path.Combine
                (Directory.GetCurrentDirectory(), "wwwroot", "Report", report.Id.ToString());
            if (!Directory.Exists(directoryPath))
                Directory.Delete(directoryPath, true);
            bool deletionResult = Convert.ToBoolean(await _unitOfWork.SaveChangesAsync());
            return deletionResult;
        }
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _reports.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await _reports.IgnoreQueryFilters().ToListAsync();
        }
        // Implement other interface methods as needed
    }


}
