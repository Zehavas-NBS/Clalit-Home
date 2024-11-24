using EmployeeManagerAPI.Models.Requests;
using EmployeeManagerAPI.Models.Responses;
using EmployeeManagerAPI.Models;
using System.Security.Claims;

namespace EmployeeManagerAPI.Services
{
    public interface IEmployeeService
    {
        Task<ICollection<GetEmployeesResponse>> GetEmployeesByManagerId(ClaimsPrincipal user);
        Task<CRUDEmployeeResponse> AddEmployee(AddEmployeeRequest request, ClaimsPrincipal user);
        Task<bool> DeleteEmployee(Guid id);
        Task<Employee> EditEmployee(EditEmployeeRequest employee);

    }
}
