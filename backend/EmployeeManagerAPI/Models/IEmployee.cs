namespace EmployeeManagerAPI.Models
{
    public interface IEmployee
    {
         Guid Id { get; set; } 
         string Email { get; set; } 
         string Password { get; set; } 
         string FullName { get; set; }
         DateTime CreatedAt { get; set; } 
    }
}
