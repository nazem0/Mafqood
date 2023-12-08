using AutoMapper;
using Domain.DTOs;
using Domain.DTOs.ReportDTOs;
using Domain.Interfaces.IRepositories;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReportRepository _reportRepository;
        public ReportsController(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        // GET: api/Reports
        [HttpGet("{pageIndex}")]
        public ActionResult<IEnumerable<ReportViewDTO>> GetReports(int pageIndex, [FromQuery] ReportFiltrationDTO filter, int pageSize = 5)
        {
            PaginationViewDTO<ReportViewDTO> reportsList =
                _reportRepository
                .Get(filter)
                .OrderByDescending(r => r.DateTime)
                .ToPaginationViewDTO(pageIndex, pageSize, _mapper.Map<ReportViewDTO>);
            return Ok(reportsList);
        }

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<ReportViewDTO>> GetAll()
        {
            IEnumerable<ReportViewDTO> reportsList = _reportRepository.GetAll().Select(_mapper.Map<ReportViewDTO>);
            return Ok(reportsList);
        }


    }
}
