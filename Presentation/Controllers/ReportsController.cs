﻿using Application.DTOs.ReportDTOs;
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
        public ActionResult<PaginationViewDTO<ReportViewDTO>> GetReports(int pageIndex, [FromQuery] ReportFiltrationDTO filter, int pageSize = 5)
        {
            return _reportRepository.Get(pageIndex, pageSize, filter);
        }

        [Authorize]
        [HttpGet("GetAll/{pageIndex}")]
        public ActionResult<PaginationViewDTO<ReportViewDTO>> GetAll(int pageIndex, [FromQuery] ReportFiltrationDTO filter, int pageSize = 5)
        {
            return _reportRepository.GetAll(pageIndex, pageSize, filter);
        }

        [Authorize]
        [HttpGet("GetUnvalid/{pageIndex}")]
        public ActionResult<PaginationViewDTO<ReportViewDTO>> GetUnvalid(int pageIndex, int pageSize = 5)
        {
            return _reportRepository.GetUnvalid(pageIndex, pageSize);
        }

    }
}
