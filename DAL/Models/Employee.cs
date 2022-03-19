using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Employees")]
    public class Employee
    {
        public Employee(Guid id, string name)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "Id is invalid");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Name is invalid");
            }
            
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        
        public string Name { get; set; }
    }
}