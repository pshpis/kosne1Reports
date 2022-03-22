using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;
using Server.ReportsExceptions.Specific;
using TaskStatus = DAL.Models.TaskStatus;

namespace Server.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskModel> Create(string name, string description);

        List<TaskModel> GetAll();

        Task<List<TaskModel>> GetByExecutor(Guid employeeId);

        Task<List<TaskModel>> GetByChanger(Guid changerId);

        Task<TaskModel?> FindById(Guid id);

        Task<TaskModel> SetExecutor(Guid taskId, Guid executorId);

        Task ChangeStatus(Guid taskId, TaskStatus newStatus, Guid changerId);

        Task Remove(Guid id);

        Task Update(TaskModel taskModel);

        Task<TaskChangeModel> ChangeDescription(Guid taskId, string newDescription, Guid employeeId);

        Task<TaskCommentModel> WriteComment(Guid taskId, Guid employeeId, string text);

        List<TaskCommentModel> GetAllComments();
    }
}