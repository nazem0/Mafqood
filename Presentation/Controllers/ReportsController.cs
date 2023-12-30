using Application.DTOs.ReportDTOs;
using Application.Interfaces.IRepositories;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        public ReportsController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        // GET: api/Reports
        [HttpGet("{pageIndex}")]
        public async Task<ActionResult<PaginationViewDTO<ReportViewDTO>>> GetReportsAsync(int pageIndex, [FromQuery] ReportFiltrationDTO filter, int pageSize = 5)
        {
            return await _reportRepository.GetAsync(pageIndex, pageSize, filter);
        }

        [Authorize]
        [HttpGet("GetAll/{pageIndex}")]
        public async Task<ActionResult<PaginationViewDTO<ReportViewDTO>>> GetAllAsync(int pageIndex, [FromQuery] ReportFiltrationDTO filter, int pageSize = 5)
        {
            return await _reportRepository.GetAllAsync(pageIndex, pageSize, filter);
        }

    }
}
