using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Enums;
using DietAssistant.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DietAssistant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private IUserService _userService;
        private IReportService _reportService;

        public CustomersController(IUserService userService, IReportService reportService)
        {
            _userService = userService;
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _userService.GetCustomersAsync();
        } 

        [HttpGet("{id}/dailyreport")]
        public async Task<DailyReportDTO> GetDailyReport(int id, DateTime reportDate)
        {
            return await _reportService.GetDailyReportAsync(id, reportDate);
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer(Customer customer)
        {
            var result = await _userService.AddNewUserAsync(customer, UserRole.Customer);

            return Ok(new { id = result });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                ModelState.AddModelError("updError", "Id in the path does not match with model id!");
                return BadRequest();
            }

            await _userService.UpdateCustomerAsync(customer);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            await _userService.DeleteCustomerAsync(id);

            return NoContent();
        }
    }
}
