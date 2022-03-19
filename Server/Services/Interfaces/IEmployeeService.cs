using System;
using System.Threading.Tasks;
using DAL.Models;

namespace Server.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee> Create(string name);

        Employee? FindById(Guid id);

        void Delete(Guid id);

        Employee Update(Employee entity);
    }
}