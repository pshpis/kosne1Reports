using System;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.ModelsExceptions;

namespace DAL.Models
{
    [Table("Changes")]
    public class TaskChangeModel
    {
        private DateTime _changingDate;
        public TaskChangeModel()
        {
        }
        
        public TaskChangeModel(Guid id, TaskModel task, EmployeeModel employee, DateTime changingDate)
        {
            if (id == Guid.Empty)
                throw new CreatingException(nameof(id), "Id is invalid");
            
            Id = id;
            Task = task;

            if (changingDate == default)
                throw new CreatingException(nameof(changingDate), "Date is invalid");
            
            _changingDate = changingDate.ToUniversalTime();
            Employee = employee;
        }

        public Guid Id { get; set; }
        public TaskModel Task { get; set; }

        public DateTime ChangingDate
        {
            get => _changingDate;
            set => _changingDate = value.ToUniversalTime();

        }
        public EmployeeModel Employee { get; set; }
    }
}