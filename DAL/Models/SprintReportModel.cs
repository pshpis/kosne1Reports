using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.ModelsExceptions;

namespace DAL.Models
{
    [Table("SprintReports")]
    public class SprintReportModel
    {
        private DateTime _creationDate;
        public Guid Id { get; set; }
        public List<ReportModel> Reports { get; set; }
        public EmployeeModel Employee { get; set; }
        public DateTime CreationDate { get => _creationDate; set => _creationDate = value.ToUniversalTime(); }

        public SprintReportModel()
        {
        }

        public SprintReportModel(Guid id, List<ReportModel> reports, EmployeeModel employee, DateTime creationDate)
        {
            if (id == Guid.Empty)
            {
                throw new CreatingException(nameof(id), "Id is invalid");
            }
            
            Id = id;
            Reports = reports;
            Employee = employee;
            _creationDate = creationDate.ToUniversalTime();
        }
    }
}