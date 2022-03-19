using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using Newtonsoft.Json;
using Server.DbContexts;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class EmployeeService : IEmployeeService
    {
        private const string dbPath = "employees.json";
        private readonly ReportsDbContext _context;

        public EmployeeService(ReportsDbContext context) {
            _context = context;
        }

        public async Task<Employee> Create(string name)
        {
            var employee = new Employee(Guid.NewGuid(), name);
            var employeeFromDb = await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public Employee? FindById(Guid id)
        {
            var fakeGuid = Guid.Parse("ac8ac3ce-f738-4cd6-b131-1aa0e16eaadc");
            return id == fakeGuid ? new Employee(fakeGuid, "Abobus") : null;
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Employee Update(Employee entity)
        {
            throw new NotImplementedException();
        }
    }
}