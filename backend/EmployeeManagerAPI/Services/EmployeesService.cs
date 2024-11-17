using EmployeeManagerAPI.Data;
using EmployeeManagerAPI.Models;
using EmployeeManagerAPI.Models.Requests;
using EmployeeManagerAPI.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeeManagerAPI.Services
{
    public class EmployeesService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public EmployeesService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ICollection<GetEmployeesResponse>> GetEmployeesByManagerId(ClaimsPrincipal user)
        {
            var managerIdClaim = user.FindFirst(JwtRegisteredClaimNames.Jti);
            //if (managerIdClaim == null)
            //{
            //    return Unauthorized(new { message = "Unauthorized access." });
            //}

            Guid managerId = Guid.Parse(managerIdClaim.Value);
            Manager manager = await _context.Managers.FirstOrDefaultAsync(u => u.Id == managerId);
            var employees = await _context.Employees.Where(u => u.ManagerId == manager.Id).Select(emp=>
                new GetEmployeesResponse()
                {
                    Email = emp.Email,
                    FullName = emp.FullName,
                    Id = emp.Id,
                    Password = emp.Password
                }
                ).ToListAsync();

            return employees;

        }

        internal async Task<CRUDEmployeeResponse> AddEmployee(BaseEntity employee, ClaimsPrincipal user)
        {
            employee.CreatedAt = DateTime.UtcNow;
            var managerIdClaim = user.FindFirst(JwtRegisteredClaimNames.Jti);
            //if (managerIdClaim == null)
            //{
            //    return Unauthorized(new { message = "Unauthorized access." });
            //}
            Guid managerId = Guid.Parse(managerIdClaim.Value);

            Employee newEmployee = new Employee();
            newEmployee.FullName = employee.FullName;
            newEmployee.CreatedAt = employee.CreatedAt;
            newEmployee.Password = employee.Password;
            newEmployee.Email = employee.Email;

            newEmployee.ManagerId = managerId;
            //employee.Id = 
            var data = _context.Employees.Add(newEmployee);
            await _context.SaveChangesAsync();
            return new CRUDEmployeeResponse() {EmployeeData = newEmployee };
        }

        internal async Task<Employee?> DeleteEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return null;

            var data = _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return data.Entity;
        }

        internal async Task<Employee> EditEmployee(BaseEntity employee)
        {
            var exsitingEmployee = await _context.Employees.FindAsync(employee);

                if (exsitingEmployee == null)
                {
                    throw new Exception("Employee not found");
                }


            // עדכון שדות העובד
            exsitingEmployee.Email = employee.Email;
            exsitingEmployee.FullName = employee.FullName;
            exsitingEmployee.Password = employee.Password; // הנח כי הסיסמה כבר מוצפנת
            exsitingEmployee.CreatedAt = employee.CreatedAt;

            var data =  _context.Employees.Update(exsitingEmployee);
            await _context.SaveChangesAsync();
            return data.Entity;
        }
    }
    }
