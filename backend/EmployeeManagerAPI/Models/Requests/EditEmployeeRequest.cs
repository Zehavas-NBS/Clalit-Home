namespace EmployeeManagerAPI.Models.Requests
{
    public class EditEmployeeRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }
}
