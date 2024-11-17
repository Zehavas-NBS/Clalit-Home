namespace EmployeeManagerAPI.Models.Requests
{
    public class ManagerSignupRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
