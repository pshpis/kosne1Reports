using System;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.ModelsExceptions;

namespace DAL.Models
{
    [Table("Tasks")]
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EmployeeModel? Executor { get; set; }
        public DateTime CreationDate { get; set; }
        
        public TaskStatus Status { get; set; }

        public TaskModel()
        {
        }

        public TaskModel(Guid id, string name, string description, TaskStatus status = TaskStatus.Open, EmployeeModel? executor = null, DateTime creationDate = default)
        {
            if (id == Guid.Empty)
                throw new CreatingException(nameof(id), "Id is invalid");

            if (string.IsNullOrWhiteSpace(name))
                throw new CreatingException(nameof(name), "Name is invalid");

            if (string.IsNullOrWhiteSpace(description))
                throw new CreatingException(nameof(description), "Description is invalid");
            
            Id = id;
            Name = name;
            Description = description;
            Status = status;
            Executor = executor;
            CreationDate = creationDate;
        }
    }
}