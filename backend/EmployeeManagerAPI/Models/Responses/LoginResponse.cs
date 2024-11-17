namespace EmployeeManagerAPI.Models.Responses
{
    public class LoginResponse
    {
        public LoginResponse(string token, BaseEntity existingUser)
        {
            Token = token;
            ManagerData = existingUser;
        }

        public string Token { get; }
        public BaseEntity ManagerData { get; }
    }
}
