using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.ModelsExceptions;

namespace DAL.Models
{
    [Table("Reports")]
    public class ReportModel
    {
        public ReportModel()
        {
        }

        public ReportModel(Guid id, EmployeeModel employee, DateTime dateTime, List<TaskModel> tasks, bool atCurrentSprint = true)
        {
            if (id == Guid.Empty)
            {
                throw new CreatingException(nameof(id), "Id is invalid");
            }

            Id = id;
            Employee = employee;
            DateTime = dateTime;
            Tasks = tasks;
            AtCurrentSprint = atCurrentSprint;
        }

        public Guid Id { get; set; }
        public EmployeeModel Employee { get; set; }
        public DateTime DateTime { get; set; }
        public List<TaskModel> Tasks { get; set; }
        public bool AtCurrentSprint { get; set; }
    }
}