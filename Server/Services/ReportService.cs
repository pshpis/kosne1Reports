using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Server.DbContexts;
using Server.ReportsExceptions.Specific;
using Server.Services.Interfaces;
using TaskStatus = DAL.Models.TaskStatus;

namespace Server.Services
{
    public class ReportService : IReportService
    {
        private readonly ReportsDbContext _context;
        private readonly IEmployeeService _employeeService;
        private readonly ITaskService _taskService;

        public ReportService(ReportsDbContext context, IEmployeeService employeeService, ITaskService taskService)
        {
            _context = context;
            _employeeService = employeeService;
            _taskService = taskService;
        }

        public async Task<ReportModel> Create(Guid employeeId)
        {
            var employee = await _employeeService.FindById(employeeId);
            if (employee == null) throw new WrongIdException("Wrong employee id");
            var tasks = await _taskService.GetFinishedByExecutor(employeeId);
            foreach (var task in tasks)
            {
                task.Status = TaskStatus.Closed;
                await _taskService.Update(task);
            }
            
            ReportModel report = new ReportModel(Guid.NewGuid(), employee, DateTime.Now, tasks);
            await _context.Reports.AddAsync(report);
            return report;
        }

        public async Task<ReportModel?> FindById(Guid id)
        {
            return await _context.Reports.Include(r => r.Employee)
                .Include(r => r.Tasks)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public List<ReportModel> GetCurrents()
        {
            return _context.Reports.Where(r => r.AtCurrentSprint).ToList();
        }
        public List<ReportModel> GetAll()
        {
            return _context.Reports.ToList();
        }

        public async Task Update(ReportModel report)
        {
            _context.Reports.Update(report);
            await _context.SaveChangesAsync();
        }
        public async Task<ReportModel> AddTask(Guid reportId, Guid taskId, Guid employeeId)
        {
            ReportModel? report = await FindById(reportId);
            if (report == null) throw new WrongIdException("Wrong report id");
            TaskModel? task = await _taskService.FindById(taskId);
            if (task == null) throw new WrongIdException("Wrong task exception");
            EmployeeModel? employee = await _employeeService.FindById(employeeId);
            if (employee == null) throw new WrongIdException("Wrong employee id");

            if (!await _employeeService.IsInSquad(employeeId, report.Employee.Id))
                throw new NotEnoughRightsException("Can't add task to this report");

            if (task.Status != TaskStatus.Finished)
                throw new ForbiddenActionException("Try to make report of not finished task");
            
            report.Tasks.Add(task);
            task.Status = TaskStatus.Closed;
            await _taskService.Update(task);
            await Update(report);
            return report;
        }
    }
}