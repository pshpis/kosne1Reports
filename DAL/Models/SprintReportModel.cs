using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.ModelsExceptions;

namespace DAL.Models
{
    [Table("SprintReports")]
    public class SprintReportModel
    {
        public Guid Id { get; set; }
        public List<ReportModel> Reports { get; set; }
        public EmployeeModel Employee { get; set; }
        public DateTime CreationDate;

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
            CreationDate = creationDate;
        }
    }
}