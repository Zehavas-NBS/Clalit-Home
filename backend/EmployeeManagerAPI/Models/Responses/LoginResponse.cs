namespace EmployeeManagerAPI.Models.Responses
{
    public class LoginResponse
    {
        public LoginResponse(string token, IEmployee existingUser)
        {
            Token = token;
            ManagerData = existingUser;
        }

        public string Token { get; }
        public IEmployee ManagerData { get; }
    }
}
