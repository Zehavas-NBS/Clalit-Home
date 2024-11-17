namespace EmployeeManagerAPI.Models.Responses
{
    public class GetEmployeesResponse
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // מזהה ייחודי (GUID)
        public string Email { get; set; } = string.Empty; // כתובת מייל
        public string Password { get; set; } = string.Empty; // סיסמה (תישמר כהאש)
        public string FullName { get; set; } = string.Empty; // שם מלא
    }
}
