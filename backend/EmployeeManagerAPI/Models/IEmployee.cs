namespace EmployeeManagerAPI.Models
{
    public interface IEmployee
    {
        //TODO : Zehava - Change this field to primary key ValueGeneratedOnAdd - Auto increment field
        //public int EntityId { get; set; }
        Guid Id { get; set; } 
         string Email { get; set; } 
         string Password { get; set; } 
         string FullName { get; set; }
         DateTime CreatedAt { get; set; } 
    }
}
