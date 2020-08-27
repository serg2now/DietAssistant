using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DietAssistant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumeLogController : ControllerBase
    {
        private IConsumedDishService _consumedDishService;

        public ConsumeLogController(IConsumedDishService consumedDishService)
        {
            _consumedDishService = consumedDishService;
        }

        [HttpGet]
        public async Task<IEnumerable<ConsumeLogItem>> GetLogItems(int customerId, DateTime consumeDate)
        {
            return await _consumedDishService.GetLogItemsAsync(customerId, consumeDate);
        }

        [HttpPost]
        public async Task<ActionResult> AddLogItem(ConsumeLogItem logItem)
        {
            var result = await _consumedDishService.AddLogItemAsync(logItem);

            return Ok(new { result });
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLogItem(int id, ConsumeLogItem logItem)
        {
            if (id != logItem.Id)
            {
                ModelState.AddModelError("upd_err", "Id in the path does not match with model id!");
                return BadRequest();
            }

            await _consumedDishService.UpdateLogItem(logItem);

            return NoContent();
        }
    }
}
