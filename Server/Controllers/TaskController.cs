using System;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Server.ReportsExceptions;
using Server.Services.Interfaces;
using TaskStatus = DAL.Models.TaskStatus;

namespace Server.Controllers
{
    [ApiController]
    [Route("/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }
        
        [HttpPost]
        public async Task<TaskModel> Create([FromQuery] string name, [FromQuery] string description)
        {
            return await _service.Create(name, description);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid id, [FromQuery] Guid executorId, [FromQuery] Guid changerId)
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

            if (executorId != Guid.Empty)
            {
                try
                {
                    var task = await _service.GetByExecutor(executorId);
                    return Ok(task);
                }
                catch (ReportsGlobalException e)
                {
                    return BadRequest(e.Message);
                }
            }

            if (changerId != Guid.Empty)
            {
                try
                {
                    var task = await _service.GetByChanger(changerId);
                    return Ok(task);
                }
                catch (ReportsGlobalException e)
                {
                    return BadRequest(e.Message);
                }
            }
            
            return Ok(_service.GetAll());
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromQuery] Guid id)
        {
            await _service.Remove(id);
            return Ok();
        }

        [HttpPatch("executor")]
        public async Task<IActionResult> SetExecutor([FromQuery] Guid taskId, [FromQuery] Guid executorId)
        {
            try
            {
                var task = await _service.SetExecutor(taskId, executorId);
                return Ok(task);
            }
            catch (ReportsGlobalException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPatch("finish")]
        public async Task<IActionResult> FinishTask([FromQuery] Guid taskId, [FromQuery] Guid changerId)
        {
            try
            {
                await _service.ChangeStatus(taskId, TaskStatus.Finished, changerId);
                return Ok(_service.FindById(taskId));
            }
            catch (ReportsGlobalException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("description")]
        public async Task<IActionResult> SetDescription([FromQuery] Guid taskId, [FromQuery] string newDescription, [FromQuery] Guid employeeId)
        {
            try
            {
                var taskChange = await _service.ChangeDescription(taskId, newDescription, employeeId);
                return Ok(taskChange);
            }
            catch (ReportsGlobalException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("comment")]
        public async Task<IActionResult> AddComment([FromQuery] Guid taskId, [FromQuery] Guid employeeId, [FromQuery] string text)
        {
            try
            {
                var comment = await _service.WriteComment(taskId, employeeId, text);
                return Ok(comment);
            }
            catch (ReportsGlobalException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("comments")]
        public async Task<IActionResult> GetAllComments()
        {
            return Ok(_service.GetAllComments());
        }
    }
}