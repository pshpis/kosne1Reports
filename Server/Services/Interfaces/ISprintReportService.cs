using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace Server.Services.Interfaces
{
    public interface ISprintReportService
    {
        Task<SprintReportModel> Create(Guid employeeId);

        List<SprintReportModel> GetAll();

        Task<SprintReportModel?> FindById(Guid id);
    }
}