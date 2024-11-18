namespace EmployeeManagerAPI.Models
{
    public class Employee : EmployeeBase
    {
        public Employee(string email, string fullName, string password, Guid managerId) : base(email, fullName, password)
        {
            ManagerId = managerId;
        }

        public Guid ManagerId { get; set; }
        public Manager Manager { get; set; }
    }
}
