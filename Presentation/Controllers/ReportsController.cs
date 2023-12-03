using AutoMapper;
using Domain.DTOs.ReportDTOs;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly EntitiesContext _context;
        public ReportsController(EntitiesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Reports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportViewDTO>>> GetReports()
        {
            if (_context.Reports == null)
            {
                return NotFound();
            }
            return await _context.Reports.Select(r => _mapper.Map<ReportViewDTO>(r)).ToListAsync();
        }

        // GET: api/Reports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReportViewDTO>> GetReport(Guid id)
        {
            if (_context.Reports == null)
            {
                return NotFound();
            }
            var report = await _context.Reports.FindAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            return _mapper.Map<ReportViewDTO>(report);
        }
        #region Update Action (NOT USED)
        // PUT: api/Reports/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutReport(Guid id, Report report)
        //{
        //    if (id != report.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(report).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ReportExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}
        #endregion
        // POST: api/Reports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReportViewDTO>> PostReport([FromForm]ReportAdditionDTO Report)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.SelectMany(ms => ms.Value.Errors.Select(e => new { Field = ms.Key, Error = e.ErrorMessage })).ToList();
                return BadRequest(errorList);
            }
            Report? createdReport = _mapper.Map<Report>(Report);
            //Directory Existence Checking
            string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Report", createdReport.Id.ToString());
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);
            await _context.Reports.AddAsync(createdReport);
            foreach (IFormFile file in Report.ReportAttachments)
            {
                FileInfo fi = new(file.FileName);
                string fileName = DateTime.Now.Ticks + fi.Extension;
                string filePath = Path.Combine(DirectoryPath, fileName);
                FileStream fileStream = System.IO.File.Create(filePath);
                await file.CopyToAsync(fileStream);
                createdReport.Attachments.Add(new Attachment
                {
                    Name = fileName,
                    ReportId = createdReport.Id
                });
            }
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ReportViewDTO>(createdReport));
        }

        // DELETE: api/Reports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(Guid id)
        {
            if (_context.Reports == null)
            {
                return NotFound();
            }
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReportExists(Guid id)
        {
            return (_context.Reports?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
