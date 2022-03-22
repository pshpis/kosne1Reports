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
    public class TaskService : ITaskService
    {
        private readonly ReportsDbContext _context;
        private readonly IEmployeeService _employeeService;

        public TaskService(ReportsDbContext context, IEmployeeService employeeService)
        {
            _context = context;
            _employeeService = employeeService;
        }

        public async Task<TaskModel> Create(string name, string description)
        {
            TaskModel newTask = new TaskModel(Guid.NewGuid(), name, description);
            await _context.Tasks.AddAsync(newTask);
            await _context.SaveChangesAsync();
            return newTask;
        }

        public List<TaskModel> GetAll()
        {
            return _context.Tasks.ToList();
        }

        public async Task<List<TaskModel>> GetByExecutor(Guid employeeId)
        {
            var employee = await _employeeService.FindById(employeeId);
            if (employee == null) throw new WrongIdException("Wrong employee id");

            return _context.Tasks.ToList()
                .Where(task => task.Executor != null && task.Executor.Id == employeeId).ToList();
        }

        public async Task<List<TaskModel>> GetFinishedByExecutor(Guid employeeId)
        {
            var employee = await _employeeService.FindById(employeeId);
            if (employee == null) throw new WrongIdException("Wrong employee id");

            return _context.Tasks.ToList()
                .Where(task => task.Executor != null
                    && task.Executor.Id == employeeId
                    && task.Status == TaskStatus.Finished).ToList();
        }

        public async Task<List<TaskModel>> GetByChanger(Guid changerId)
        {
            var employee = await _employeeService.FindById(changerId);
            if (employee == null) throw new WrongIdException("Wrong employee id");

            var changes = _context.Changes.Where(change => change.Employee.Id == changerId).ToList();
            HashSet<Guid> tasksIds = new HashSet<Guid>() ;
            changes.ForEach(change => tasksIds.Add(change.Task.Id));

            List<TaskModel> tasks = new List<TaskModel>();
            foreach (var taskId in tasksIds)
            {
                tasks.Add(await FindById(taskId));
            }

            return tasks;
        }

        public async Task<TaskModel?> FindById(Guid id)
        {
            return await _context.Tasks.Include(t => t.Executor)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskModel> SetExecutor(Guid taskId, Guid executorId)
        {
            var task = await FindById(taskId);
            if (task == null) throw new WrongIdException("Wrong task id");

            var employee = await _employeeService.FindById(executorId);
            if (employee == null) throw new WrongIdException("Wrong employee id");

            if (task.Status != TaskStatus.Open) throw new ForbiddenActionException("This task already has executor");
            task.Executor = employee;
            await Update(task);
            return task;
        }

        public async Task ChangeStatus(Guid taskId, TaskStatus newStatus, Guid changerId)
        {
            var task = await FindById(taskId);
            if (task == null) throw new WrongIdException("Wrong task id");

            var employee = await _employeeService.FindById(changerId);
            if (employee == null) throw new WrongIdException("Wrong employee id");

            if (newStatus == TaskStatus.Closed)
                throw new ForbiddenActionException("Can't set closed status to task");
            
            if (!employee.IsTeamLead() &&
                task.Executor != null
                && !await _employeeService.IsInSquad(employee.Id, task.Executor.Id))
                throw new NotEnoughRightsException("This changer don't have enough rights");

            task.Status = newStatus;
            await Update(task);
        }
        public async Task Remove(Guid id)
        {
            var task = await FindById(id);
            if (task == null) return;
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TaskModel taskModel)
        {
            _context.Tasks.Update(taskModel);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskChangeModel> ChangeDescription(Guid taskId, string newDescription, Guid employeeId)
        {
            var task = await FindById(taskId);
            if (task == null) throw new WrongIdException("Wrong task id");

            var employee = await _employeeService.FindById(employeeId);
            if (employee == null) throw new WrongIdException("Wrong employee id");

            var change = await RegisterChange(employee, task);
            
            task.Description = newDescription;
            await Update(task);
            
            return change;
        }

        public async Task<TaskCommentModel> WriteComment(Guid taskId, Guid employeeId, string text)
        {
            var task = await FindById(taskId);
            if (task == null) throw new WrongIdException("Wrong task id");

            var employee = await _employeeService.FindById(employeeId);
            if (employee == null) throw new WrongIdException("Wrong employee id");

            var change = await RegisterChange(employee, task);

            TaskCommentModel comment = new TaskCommentModel(Guid.NewGuid(), change, text);
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public List<TaskCommentModel> GetAllComments() => _context.Comments.ToList();
        private async Task<TaskChangeModel> RegisterChange(EmployeeModel employee, TaskModel task)
        {
            TaskChangeModel change = new TaskChangeModel(Guid.NewGuid(), task, employee, DateTime.Now);
            await _context.Changes.AddAsync(change);
            await _context.SaveChangesAsync();
            return change;
        }
    }
}