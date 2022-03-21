using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using Server.ReportsExceptions;

namespace Server.Services.Interfaces
{
    public interface IEmployeeService
    {
        List<EmployeeModel> GetAll();
        Task<EmployeeModel> Create(string name);

        Task<EmployeeModel?> FindById(Guid id);

        Task Delete(Guid id);

        Task Update(EmployeeModel entity);

        Task CreateLink(Guid bossId, Guid employeeId);

        Task<List<EmployeeModel>> GetSlavesByBoss(Guid bossId);

        Task<List<EmployeeModel>> GetSquadList(Guid bossId);
        Task<List<EmployeeModel>> GetBosses(Guid employeeId);
    }
}