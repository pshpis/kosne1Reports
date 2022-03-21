using System;
using DAL.ModelsExceptions;

namespace DAL.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EmployeeModel? Executor { get; set; }
        public DateTime CreationDate { get; set; }

        public TaskModel()
        {
        }

        public TaskModel(Guid id, string name, string description, EmployeeModel? executor = null, DateTime creationDate = default)
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
            Executor = executor;
            CreationDate = creationDate;
        }
    }
}