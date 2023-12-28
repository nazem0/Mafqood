using Application.DTOs.ReportDTOs;
using Domain.Entities;
using Application.Interfaces.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        //GET: api/Reports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReportViewDTO>> GetReport(Guid id)
        {
            ReportViewDTO? report = await _reportRepository.GetByIdAsync(id);
            return report is null ? NotFound() : Ok(report);
        }

        // POST: api/Reports
        [HttpPost]
        public async Task<IActionResult> PostReport([FromForm] ReportAdditionDTO report)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState
                    .SelectMany(ms => ms.Value!.Errors
                    .Select(e => new { Field = ms.Key, Error = e.ErrorMessage }))
                    .ToList();
                return BadRequest(errorList);
            }

            HttpStatusCode reportAddition = await _reportRepository.AddAsync(report);
            return StatusCode((int)reportAddition);
        }

        // DELETE: api/Reports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteReport(Guid id, string deletionCode)
        {
            HttpStatusCode reportDeletion = await _reportRepository.SoftDeleteAsync(id, deletionCode);
            return StatusCode((int)reportDeletion);
        }

        [HttpDelete("Delete/{id}"), Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            HttpStatusCode reportDeletion = await _reportRepository.DeleteAsync(id);
            return StatusCode((int)reportDeletion);
        }

        //APPROVE: api/Reports/5
        [HttpPatch("Approve/{id}"), Authorize(Roles = "admin")]
        public async Task<IActionResult> Approve(Guid id)
        {
            HttpStatusCode reportApproval = await _reportRepository.ApproveAsync(id);
            return StatusCode((int)reportApproval);
        }
    }
}
