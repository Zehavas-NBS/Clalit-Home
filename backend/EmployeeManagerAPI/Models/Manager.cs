namespace EmployeeManagerAPI.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class Manager : EmployeeBase
    {
        public Manager(string email, string fullName, string password) : base(email, fullName, password)
        {
        }

        [JsonIgnore]

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }

}
