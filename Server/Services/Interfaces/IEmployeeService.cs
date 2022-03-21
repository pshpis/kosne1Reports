using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using Server.ReportsExceptions;

namespace Server.Services.Interfaces
{
    public interface IEmployeeService
    {
        List<Employee> GetAll();
        Task<Employee> Create(string name);

        Task<Employee?> FindById(Guid id);

        Task Delete(Guid id);

        Task Update(Employee entity);

        Task CreateLink(Guid bossId, Guid employeeId);

        Task<List<Employee>> GetSlavesByBoss(Guid bossId);

        Task<List<Employee>> GetSquadList(Guid bossId);
        Task<List<Employee>> GetBosses(Guid employeeId);
    }
}