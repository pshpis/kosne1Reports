using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using Server.ReportsExceptions.Specific;
using TaskStatus = DAL.Models.TaskStatus;

namespace Server.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReportModel> Create(Guid employeeId);

        Task<ReportModel?> FindById(Guid id);

        List<ReportModel> GetCurrents();
        List<ReportModel> GetAll();
        Task Update(ReportModel report);

        Task<ReportModel> AddTask(Guid reportId, Guid taskId, Guid employeeId);
    }
}