using EmployeeManagerAPI.Models.Requests;
using EmployeeManagerAPI.Models;
using EmployeeManagerAPI.Models.Responses;

namespace EmployeeManagerAPI.Services
{
    public interface IAuthService
    {
        Task<IEmployee> SignUpAsync(ManagerSignupRequest request);
        Task<LoginResponse> LoginAsync(LogInRequest request);
    }
}
