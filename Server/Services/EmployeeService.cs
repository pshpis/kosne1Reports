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

        public List<EmployeeModel> GetAll()
        {
            return _context.Employees.ToList();
        }
        public async Task<EmployeeModel> Create(string name)
        {
            var employee = new EmployeeModel(Guid.NewGuid(), name);
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<EmployeeModel?> FindById(Guid id)
        {
            return await _context.Employees.Include(e => e.Boss)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Delete(Guid id)
        {
            var employee = await FindById(id);
            if (employee == null) return;
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task Update(EmployeeModel entity)
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

        public async Task<List<EmployeeModel>> GetSlavesByBoss(Guid bossId)
        {
            var boss = await FindById(bossId);
            if (boss == null) throw new WrongIdException("Wrong boss id");
            var slaves = _context.Employees.ToList().Where(employee =>  !employee.IsTeamLead() && employee.Boss!.Id == bossId).ToList();
            return slaves;
        }

        public async Task<List<EmployeeModel>> GetSquadList(Guid bossId)
        {
            var boss = await FindById(bossId);
            if (boss == null) throw new WrongIdException("Wrong boss id");
            
            List<EmployeeModel> squad = new() { boss };
            for (var i = 0; i < squad.Count; i++)
            {
                var employee = squad[i].Boss;
                if (employee != null) squad.AddRange(await GetSlavesByBoss(employee.Id));
            }

            return squad;
        }

        public async Task<bool> IsInSquad(Guid bossId, Guid slaveId)
        {
            List<EmployeeModel> squad = await GetSquadList(bossId);
            return squad.Any(e => e.Id == slaveId);
        }

        public async Task<List<EmployeeModel>> GetBosses(Guid employeeId)
        {
            var employee = await FindById(employeeId);
            if (employee == null) throw new WrongIdException("Wrong employee id");
            if (employee.IsTeamLead()) return new List<EmployeeModel>();

            List<EmployeeModel> bosses = new();
            while (employee != null)
            {
                if (!employee.IsTeamLead())
                {
                    bosses.Add(employee.Boss!);
                    employee = await FindById(employee.Boss.Id);
                }
                else
                {
                    employee = null;
                }
            }

            return bosses;
        }

        public async Task Remove(Guid id)
        {
            var employee = await FindById(id);
            if (employee == null) return;
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}