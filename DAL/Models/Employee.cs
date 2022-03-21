using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using DAL.ModelsExceptions;

namespace DAL.Models
{
    [Table("Employees")]
    public class Employee
    {
        public Employee()
        {
        }
        
        
        public Employee(Guid id, string name, Guid bossId = default,  Employee? boss = null)
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
        
        public Employee? Boss { get; set; }

        public bool IsTeamLead()
        {
            return Boss == null;
        }
    }
}