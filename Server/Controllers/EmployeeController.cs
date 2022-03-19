using System;
using System.Net;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<Employee> Create([FromQuery] string name)
        {
            return await _service.Create(name);
        }

        [HttpGet]
        public IActionResult Find([FromQuery] Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = _service.FindById(id);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return BadRequest();
        }
    }
}