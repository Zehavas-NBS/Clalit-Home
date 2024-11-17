namespace EmployeeManagerAPI.Models
{
    public class Employee : BaseEntity
    {
        public Guid ManagerId { get; set; } // מזהה ייחודי של המנהל (Foreign Key)
        public Manager Manager { get; set; } // אובייקט מנהל (Navigation Property)
    }
}
