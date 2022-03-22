using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Server.DbContexts;
using Server.ReportsExceptions.Specific;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class SprintReportService
    {
        private readonly ReportsDbContext _context;
        private readonly IEmployeeService _employeeService;
        private readonly ITaskService _taskService;
        private readonly IReportService _reportService;

        public SprintReportService(ReportsDbContext context, IEmployeeService employeeService, 
            ITaskService taskService, IReportService reportService)
        {
            _context = context;
            _employeeService = employeeService;
            _taskService = taskService;
            _reportService = reportService;
        }

        public async Task<SprintReportModel> Create(Guid employeeId)
        {
            var employee = await _employeeService.FindById(employeeId);
            if (employee == null) throw new WrongIdException("Wrong employee id");
            if (!employee.IsTeamLead()) throw new NotEnoughRightsException("Can't create sprint report");

            var reports = _reportService.GetCurrents();
            SprintReportModel sprintReport = new SprintReportModel(Guid.NewGuid(), reports, employee, DateTime.Now);
            reports.ForEach(report =>
            {
                report.AtCurrentSprint = false;
                _reportService.Update(report);
            });
            await _context.SprintReports.AddAsync(sprintReport);
            return sprintReport;
        }
        
        public List<SprintReportModel> GetAll()
        {
            return _context.SprintReports.ToList();
        }
        
        public async Task<SprintReportModel?> FindById(Guid id)
        {
            return await _context.SprintReports.Include(r => r.Employee)
                .Include(r => r.Reports)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}