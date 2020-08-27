using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DietAssistant.API.DTOs;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Enums;
using DietAssistant.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DietAssistant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyReportsController : ControllerBase
    {
        private IAdminReportService _reportService;


        public DailyReportsController(IAdminReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IEnumerable<DailyReportDTO>> GetReports(DateTime? reportDate)
        {
            return (reportDate.HasValue)
                ? await _reportService.GetReportsByDateAsync(reportDate.Value)
                : await _reportService.GetAllReportsAsync();
        }

        [HttpGet("statistic")]
        public async Task<IEnumerable<GroupStatistic>> GetDailyStatistic(DateTime reportDate)
        {
            return await _reportService.GetUsersDailyStatisticAsync(reportDate);   
        }

        [HttpPost]
        public async Task<ActionResult> CreateReport(CreateReportModel model)
        {
            var result = await _reportService.UpsertDailyReportAsync(model.CustomerId, model.ReportDate);

            return Ok(result);
        }
    }
}
