using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.ModelsExceptions;

namespace DAL.Models
{
    [Table("Reports")]
    public class ReportModel
    {
        private DateTime _creationDate;
        public ReportModel()
        {
        }

        public ReportModel(Guid id, EmployeeModel employee, DateTime creationDate, List<TaskModel> tasks, bool atCurrentSprint = true)
        {
            if (id == Guid.Empty)
            {
                throw new CreatingException(nameof(id), "Id is invalid");
            }

            Id = id;
            Employee = employee;
            _creationDate = creationDate.ToUniversalTime();
            Tasks = tasks;
            AtCurrentSprint = atCurrentSprint;
        }

        public Guid Id { get; set; }
        public EmployeeModel Employee { get; set; }
        public DateTime CreationDate { get => _creationDate; set => _creationDate = value.ToUniversalTime(); }
        public List<TaskModel> Tasks { get; set; }
        public bool AtCurrentSprint { get; set; }
    }
}