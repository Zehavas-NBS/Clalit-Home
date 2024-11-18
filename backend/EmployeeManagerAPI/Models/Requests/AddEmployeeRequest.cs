namespace EmployeeManagerAPI.Models.Requests
{
    public class AddEmployeeRequest
    {
        public string Email { get; set; } 
        public string Password { get; set; } 
        public string FullName { get; set; } 
    }
}
