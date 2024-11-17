namespace EmployeeManagerAPI.Models
{
    using System.Collections.Generic;

    public class Manager : BaseEntity
    {
        public ICollection<Employee> Employees { get; set; } = new List<Employee>(); // רשימת העובדים תחת המנהל
    }

}
