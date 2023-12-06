using AutoMapper;
using Domain.DTOs.ReportDTOs;
using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportViewDTO>>> GetReports()
        {
            IEnumerable<Report> reports = await _reportRepository.GetAsync();
            IEnumerable<ReportViewDTO> reportsList = reports.Select(_mapper.Map<ReportViewDTO>);
            return Ok(reportsList);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ReportViewDTO>>> GetAll()
        {
            IEnumerable<Report> reports = await _reportRepository.GetAllAsync();
            IEnumerable<ReportViewDTO> reportsList = reports.Select(_mapper.Map<ReportViewDTO>);
            return Ok(reportsList);
        }

        //GET: api/Reports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReportViewDTO>> GetReport(Guid id)
        {
            Report? report = await _reportRepository.GetByIdAsync(id);

            if (report is null)
                return NotFound();

            return Ok(_mapper.Map<ReportViewDTO>(report));
        }

        // POST: api/Reports
        [HttpPost]
        public async Task<ActionResult<ReportViewDTO>> PostReport([FromForm] ReportAdditionDTO report)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.SelectMany(ms => ms.Value.Errors.Select(e => new { Field = ms.Key, Error = e.ErrorMessage })).ToList();
                return BadRequest(errorList);
            }

            bool reportAddition = await _reportRepository.AddAsync(report);
            return reportAddition ? Ok() : BadRequest();
        }

        // DELETE: api/Reports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteReport(Guid id, string deletionCode)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.SelectMany(ms => ms.Value.Errors.Select(e => new { Field = ms.Key, Error = e.ErrorMessage })).ToList();
                return BadRequest(errorList);
            }

            bool reportDeletion = await _reportRepository.SoftDeleteAsync(id, deletionCode);
            return reportDeletion ? Ok() : BadRequest();
        }
        [HttpDelete("Delete/{id}"), Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.SelectMany(ms => ms.Value.Errors.Select(e => new { Field = ms.Key, Error = e.ErrorMessage })).ToList();
                return BadRequest(errorList);
            }
            bool reportDeletion = await _reportRepository.DeleteAsync(id);
            return reportDeletion ? Ok() : BadRequest();
        }
    }
}
