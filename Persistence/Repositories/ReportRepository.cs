using Domain.Entities;
using Application.Interfaces.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Application.Interfaces;
using Application.DTOs.ReportDTOs;
using Application.ExtensionMethods;
using Domain.DTOs;
using System.Linq.Expressions;
using System.Linq;

namespace Persistence.Repositories
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

        public PaginationViewDTO<ReportViewDTO> Get(int pageIndex, int pageSize, ReportFiltrationDTO filter)
        {
            return
                _reports
                .Where(Filter(filter))
                .OrderByDescending(r => r.DateTime)
                .ToPaginationViewDTO(pageIndex, pageSize, _mapper.Map<ReportViewDTO>);
        }
        public PaginationViewDTO<ReportViewDTO> GetAll(int pageIndex, int pageSize, ReportFiltrationDTO filter)
        {
            return
                _reports
                .IgnoreQueryFilters()
                .Where(Filter(filter))
                .OrderByDescending(r => r.DateTime)
                .ToPaginationViewDTO(pageIndex, pageSize, _mapper.Map<ReportViewDTO>);
        }
        public PaginationViewDTO<ReportViewDTO> GetUnvalid(int pageIndex, int pageSize)
        {
            return
                _reports
                .Where(r => r.Valid == false)
                .OrderByDescending(r => r.DateTime)
                .ToPaginationViewDTO(pageIndex, pageSize, _mapper.Map<ReportViewDTO>);
        }

        public async Task<ReportViewDTO?> GetByIdAsync(Guid id)
        {
            Report? report = await _reports.FindAsync(id);
            return report is null ? null : _mapper.Map<ReportViewDTO>(report);
        }

        public async Task<bool> AddAsync(ReportAdditionDTO report)
        {
            Report createdReport = _mapper.Map<Report>(report);
            string directoryPath = Path.Combine
                (Directory.GetCurrentDirectory(), "wwwroot", "Report", createdReport.Id.ToString());
            #region Directory Existence Checking
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
                await fileStream.DisposeAsync();
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
            return await _reports.AnyAsync(r => r.Id == id);
        }

        private Expression<Func<Report, bool>> Filter(ReportFiltrationDTO filter)
        {
            return r =>
                (filter.Keyword != null ?
                    (
                    (r.Name != null ? (r.Name.Contains(filter.Keyword) || filter.Keyword.Contains(r.Name)) : true)
                    ||
                    (r.AdditionalInfo != null ? (r.AdditionalInfo.Contains(filter.Keyword) || filter.Keyword.Contains(r.AdditionalInfo)) : true)
                    ||
                    (r.Street != null ? (r.Street.Contains(filter.Keyword) || filter.Keyword.Contains(r.Street)) : true)
                    ||
                    (r.District != null ? (r.District.Contains(filter.Keyword) || filter.Keyword.Contains(r.District)) : true)
                    ||
                    (r.City.Governorate.NameEn.Contains(filter.Keyword) || filter.Keyword.Contains(r.City.NameEn))
                    ||
                    (r.City.Governorate.NameAr.Contains(filter.Keyword) || filter.Keyword.Contains(r.City.NameAr))
                    ||
                    (r.City.NameAr.Contains(filter.Keyword) || filter.Keyword.Contains(r.City.NameAr))
                    ||
                    (r.City.NameEn.Contains(filter.Keyword) || filter.Keyword.Contains(r.City.NameEn))
                    ) : true)
                &&
                ((filter.Name != null && r.Name != null) ? (r.Name.Contains(filter.Name) || filter.Name.Contains(r.Name)) : true) &&
                (filter.Gender != null ? r.Gender == filter.Gender : true) &&
                (filter.SkinTone != null ? r.SkinTone == filter.SkinTone : true) &&
                (filter.HairType != null ? r.HairType == filter.HairType : true) &&
                (filter.FacialHair != null ? r.FacialHair == filter.FacialHair : true) &&
                (filter.HairColor != null ? r.HairColor == filter.HairColor : true) &&
                (filter.EyeColor != null ? r.EyeColor == filter.EyeColor : true) &&
                (filter.HeightLevel != null ? r.HeightLevel == filter.HeightLevel : true) &&
                (filter.WeightLevel != null ? r.WeightLevel == filter.WeightLevel : true) &&
                (filter.MinAge != null ? r.MinAge >= filter.MinAge : true) &&
                (filter.MaxAge != null ? r.MaxAge <= filter.MaxAge : true) &&
                ((filter.Street != null && r.Street != null) ? (r.Street.Contains(filter.Street) || filter.Street.Contains(r.Street)) : true) &&
                ((filter.District != null && r.District != null) ? (r.District.Contains(filter.District) || filter.District.Contains(r.District)) : true) &&
                (filter.GovernorateId != null ? r.City.GovernorateId == filter.GovernorateId : true) &&
                (filter.CityId != null ? r.CityId == filter.CityId : true) &&
                (filter.DateTime != null ? r.DateTime >= filter.DateTime : true);
        }
        // Implement other interface methods as needed
    }


}
