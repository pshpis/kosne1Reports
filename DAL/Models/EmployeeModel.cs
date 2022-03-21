using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using DAL.ModelsExceptions;

namespace DAL.Models
{
    [Table("Employees")]
    public class EmployeeModel
    {
        public EmployeeModel()
        {
        }
        
        
        public EmployeeModel(Guid id, string name, Guid bossId = default,  EmployeeModel? boss = null)
        {
            if (id == Guid.Empty)
            {
                throw new CreatingException(nameof(id), "Id is invalid");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new CreatingException(nameof(name), "Name is invalid");
            }

            Id = id;
            Name = name;
            Boss = boss;
        }

        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public EmployeeModel? Boss { get; set; }

        public bool IsTeamLead()
        {
            return Boss == null;
        }
    }
}