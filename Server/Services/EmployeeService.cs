using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.DbContexts;
using Server.ReportsExceptions;
using Server.ReportsExceptions.Specific;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ReportsDbContext _context;

        public EmployeeService(ReportsDbContext context) {
            _context = context;
        }

        public List<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }
        public async Task<Employee> Create(string name)
        {
            var employee = new Employee(Guid.NewGuid(), name);
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> FindById(Guid id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task Delete(Guid id)
        {
            var employee = await FindById(id);
            if (employee == null) return;
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Employee entity)
        {
            _context.Employees.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task CreateLink(Guid bossId, Guid employeeId)
        {
            var boss = await FindById(bossId);
            if (boss == null) throw new WrongIdException("Wrong boss id");
            var employee = await FindById(employeeId);
            if (employee == null) throw new WrongIdException("Wrong employee id");

            employee.Boss = boss;
            await Update(employee);
        }

        public async Task<List<Employee>> GetSlavesByBoss(Guid bossId)
        {
            var boss = await FindById(bossId);
            if (boss == null) throw new WrongIdException("Wrong boss id");
            var slaves = _context.Employees.ToList().Where(employee =>  !employee.IsTeamLead() && employee.Boss!.Id == bossId).ToList();
            return slaves;
        }

        public async Task<List<Employee>> GetSquadList(Guid bossId)
        {
            var boss = await FindById(bossId);
            if (boss == null) throw new WrongIdException("Wrong boss id");
            
            List<Employee> squad = new() { boss };
            for (var i = 0; i < squad.Count; i++)
            {
                var employee = squad[i].Boss;
                if (employee != null) squad.AddRange(await GetSlavesByBoss(employee.Id));
            }

            return squad;
        }

        public async Task<List<Employee>> GetBosses(Guid employeeId)
        {
            var employee = await FindById(employeeId);
            if (employee == null) throw new WrongIdException("Wrong employee id");
            if (employee.IsTeamLead()) return new List<Employee>();

            List<Employee> bosses = new() { employee.Boss };
            while (!bosses.Last().IsTeamLead())
            {
                bosses.Add(bosses.Last().Boss);
            }

            return bosses;
        }
    }
}