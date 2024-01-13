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
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace Persistence.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Report> _reports;

        public ReportRepository
            (EntitiesContext context, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _reports = context.Set<Report>();
        }

        public async Task<PaginationViewDTO<ReportViewDTO>> GetAsync(int pageIndex, int pageSize, ReportFiltrationDTO filter)
        {
            var reports = _reports
                .Where(Filter(filter))
                .OrderByDescending(r => r.DateTime);
            return await reports.ToPaginationViewDTOAsync(pageIndex, pageSize, _mapper.Map<ReportViewDTO>);
        }
        public async Task<PaginationViewDTO<ReportViewDTO>> GetAllAsync(int pageIndex, int pageSize, ReportFiltrationDTO filter)
        {
            var reports = _reports
                .IgnoreQueryFilters()
                .Where(Filter(filter))
                .OrderByDescending(r => r.DateTime);
            return await reports.ToPaginationViewDTOAsync(pageIndex, pageSize, _mapper.Map<ReportViewDTO>);
        }

        public async Task<ReportViewDTO?> GetByIdAsync(Guid id)
        {
            Report? report = await _reports.FindAsync(id);
            return report is null ? null : _mapper.Map<ReportViewDTO>(report);
        }

        public async Task<HttpStatusCode> AddAsync(ReportAdditionDTO report)
        {
            Report createdReport = _mapper.Map<Report>(report);
            string directoryPath = Path.Combine
                (Directory.GetCurrentDirectory(), "wwwroot", "report", createdReport.Id.ToString());
            #region Directory Existence Checking
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            #endregion
            _reports.Add(createdReport);
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
            var additionResult = await _unitOfWork.SaveChangesAsync();
            return additionResult;
        }

        public async Task<HttpStatusCode> SoftDeleteAsync(Guid id, string deletionCode)
        {
            var report = await _reports.Where(r => r.Id == id && r.DeletionCode == deletionCode).FirstOrDefaultAsync();
            if (report == null) return HttpStatusCode.NotFound;
            report.Missing = false;
            _reports.Update(report);
            var deletionResult = await _unitOfWork.SaveChangesAsync();
            return deletionResult;
        }

        public async Task<HttpStatusCode> ApproveAsync(Guid id)
        {
            var report = await _reports.FindAsync(id);
            if (report is null) return HttpStatusCode.NotFound;
            report.Valid = true;
            _reports.Update(report);
            var approveResult = await _unitOfWork.SaveChangesAsync();
            return approveResult;
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid id)
        {
            var report = await _reports.FindAsync(id);
            if (report is null) return HttpStatusCode.NotFound;
            string directoryPath = Path.Combine
                (Directory.GetCurrentDirectory(), "wwwroot", "report", report.Id.ToString());
            if (!Directory.Exists(directoryPath))
                Directory.Delete(directoryPath, true);
            var deletionResult = await _unitOfWork.SaveChangesAsync();
            return deletionResult;
        }
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _reports.AnyAsync(r => r.Id == id);
        }

        private Expression<Func<Report, bool>> Filter(ReportFiltrationDTO filter)
        {
            return r =>
                (filter.Keyword == null || (
                    (r.Name == null || (r.Name.Contains(filter.Keyword) || filter.Keyword.Contains(r.Name)))
                    ||
                    (r.AdditionalInfo == null || (r.AdditionalInfo.Contains(filter.Keyword) || filter.Keyword.Contains(r.AdditionalInfo)))
                    ||
                    (r.Street == null || (r.Street.Contains(filter.Keyword) || filter.Keyword.Contains(r.Street)))
                    ||
                    (r.District == null || (r.District.Contains(filter.Keyword) || filter.Keyword.Contains(r.District)))
                    ||
                    (r.City.Governorate.NameEn.Contains(filter.Keyword) || filter.Keyword.Contains(r.City.NameEn))
                    ||
                    (r.City.Governorate.NameAr.Contains(filter.Keyword) || filter.Keyword.Contains(r.City.NameAr))
                    ||
                    (r.City.NameAr.Contains(filter.Keyword) || filter.Keyword.Contains(r.City.NameAr))
                    ||
                    (r.City.NameEn.Contains(filter.Keyword) || filter.Keyword.Contains(r.City.NameEn))
                    ))
                &&
                (filter.Valid == null || r.Valid == filter.Valid) &&
                (filter.Missing == null || r.Missing == filter.Missing) &&
                (filter.Name == null || r.Name == null || (r.Name.Contains(filter.Name) || filter.Name.Contains(r.Name))) &&
                (filter.Gender == null || r.Gender == filter.Gender) &&
                (filter.SkinTone == null || r.SkinTone == filter.SkinTone) &&
                (filter.HairType == null || r.HairType == filter.HairType) &&
                (filter.FacialHair == null || r.FacialHair == filter.FacialHair) &&
                (filter.HairColor == null || r.HairColor == filter.HairColor) &&
                (filter.EyeColor == null || r.EyeColor == filter.EyeColor) &&
                (filter.HeightLevel == null || r.HeightLevel == filter.HeightLevel) &&
                (filter.WeightLevel == null || r.WeightLevel == filter.WeightLevel) &&
                (filter.MinAge == null || r.MinAge >= filter.MinAge) &&
                (filter.MaxAge == null || r.MaxAge <= filter.MaxAge) &&
                (filter.Street == null || r.Street == null || (r.Street.Contains(filter.Street) || filter.Street.Contains(r.Street))) &&
                (filter.District == null || r.District == null || (r.District.Contains(filter.District) || filter.District.Contains(r.District))) &&
                (filter.GovernorateId == null || r.City.GovernorateId == filter.GovernorateId) &&
                (filter.CityId == null || r.CityId == filter.CityId) &&
                (filter.DateTime == null || r.DateTime >= filter.DateTime);
            #region Old Code
            //return r =>
            //    (filter.Keyword != null ?
            //        (
            //        (r.Name != null ? (r.Name.Contains(filter.Keyword) || filter.Keyword.Contains(r.Name)) : true)
            //        ||
            //        (r.AdditionalInfo != null ? (r.AdditionalInfo.Contains(filter.Keyword) || filter.Keyword.Contains(r.AdditionalInfo)) : true)
            //        ||
            //        (r.Street != null ? (r.Street.Contains(filter.Keyword) || filter.Keyword.Contains(r.Street)) : true)
            //        ||
            //        (r.District != null ? (r.District.Contains(filter.Keyword) || filter.Keyword.Contains(r.District)) : true)
            //        ||
            //        (r.City.Governorate.NameEn.Contains(filter.Keyword) || filter.Keyword.Contains(r.City.NameEn))
            //        ||
            //        (r.City.Governorate.NameAr.Contains(filter.Keyword) || filter.Keyword.Contains(r.City.NameAr))
            //        ||
            //        (r.City.NameAr.Contains(filter.Keyword) || filter.Keyword.Contains(r.City.NameAr))
            //        ||
            //        (r.City.NameEn.Contains(filter.Keyword) || filter.Keyword.Contains(r.City.NameEn))
            //        ) : true)
            //    &&
            //    ((filter.Name != null && r.Name != null) ? (r.Name.Contains(filter.Name) || filter.Name.Contains(r.Name)) : true) &&
            //    (filter.Gender != null ? r.Gender == filter.Gender : true) &&
            //    (filter.SkinTone != null ? r.SkinTone == filter.SkinTone : true) &&
            //    (filter.HairType != null ? r.HairType == filter.HairType : true) &&
            //    (filter.FacialHair != null ? r.FacialHair == filter.FacialHair : true) &&
            //    (filter.HairColor != null ? r.HairColor == filter.HairColor : true) &&
            //    (filter.EyeColor != null ? r.EyeColor == filter.EyeColor : true) &&
            //    (filter.HeightLevel != null ? r.HeightLevel == filter.HeightLevel : true) &&
            //    (filter.WeightLevel != null ? r.WeightLevel == filter.WeightLevel : true) &&
            //    (filter.MinAge != null ? r.MinAge >= filter.MinAge : true) &&
            //    (filter.MaxAge != null ? r.MaxAge <= filter.MaxAge : true) &&
            //    ((filter.Street != null && r.Street != null) ? (r.Street.Contains(filter.Street) || filter.Street.Contains(r.Street)) : true) &&
            //    ((filter.District != null && r.District != null) ? (r.District.Contains(filter.District) || filter.District.Contains(r.District)) : true) &&
            //    (filter.GovernorateId != null ? r.City.GovernorateId == filter.GovernorateId : true) &&
            //    (filter.CityId != null ? r.CityId == filter.CityId : true) &&
            //    (filter.DateTime != null ? r.DateTime >= filter.DateTime : true);
            #endregion
        }

        // Implement other interface methods as needed
    }


}
