namespace EmployeeManagerAPI.Models
{
    using System;

    public class EmployeeBase : IEmployee
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; 
        public string FullName { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public EmployeeBase(string email, string fullName, string password)
        {
            Email = email;
            FullName = fullName;
            Password = password;
        }
    }
}

