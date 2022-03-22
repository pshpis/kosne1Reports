using System;
using System.Net;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Server.ReportsExceptions;
using Server.ReportsExceptions.Specific;
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
        public async Task<EmployeeModel> Create([FromQuery] string name)
        {
            return await _service.Create(name);
        }

        [HttpGet]
        public async Task<IActionResult> Find([FromQuery] Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = await _service.FindById(id);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return Ok(_service.GetAll());
        }

        [HttpPatch("boss")]
        public async Task<IActionResult> SetBoss(Guid id, Guid bossId)
        {
            try
            {
                await _service.CreateLink(bossId, id);
                return Ok();
            }
            catch (WrongIdException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("slaves")]
        public async Task<IActionResult> GetSlaves(Guid id)
        {
            try
            {
                var slaves = await _service.GetSlavesByBoss(id);
                return Ok(slaves);
            }
            catch (ReportsGlobalException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("bosses")]
        public async Task<IActionResult> GetBosses(Guid id)
        {
            try
            {
                var bosses = await _service.GetBosses(id);
                return Ok(bosses);
            }
            catch (ReportsGlobalException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Squad")]
        public async Task<IActionResult> GetSquad(Guid id)
        {
            try
            {
                var squad = await _service.GetSquadList(id);
                return Ok(squad);
            }
            catch (ReportsGlobalException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}