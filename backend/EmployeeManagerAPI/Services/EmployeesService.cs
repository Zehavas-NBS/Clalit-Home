using EmployeeManagerAPI.Data;
using EmployeeManagerAPI.Models;
using EmployeeManagerAPI.Models.Requests;
using EmployeeManagerAPI.Models.Responses;
using log4net;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EmployeeManagerAPI.Services
{
    public class EmployeesService
    {
        private readonly AppDbContext _context;
        private readonly ILog _logger;

        public EmployeesService(IConfiguration configuration, AppDbContext context)
        {
            _logger = LogManager.GetLogger(typeof(EmployeesService));
            _context = context;
        }


        public async Task<ICollection<GetEmployeesResponse>> GetEmployeesByManagerId(ClaimsPrincipal user)
        {
            _logger.Info("Fetching all employees from the database.");

            var managerIdClaim = user.FindFirst(JwtRegisteredClaimNames.Jti);
            try
            {
                Guid managerId = Guid.Parse(managerIdClaim.Value);
                Manager manager = await _context.Managers.FirstOrDefaultAsync(u => u.Id == managerId);
                var employees = await _context.Employees.Where(u => u.ManagerId == manager.Id).Select(emp =>
                    new GetEmployeesResponse()
                    {
                        Email = emp.Email,
                        FullName = emp.FullName,
                        Id = emp.Id,
                        Password = emp.Password
                    }
                    ).ToListAsync();
                _logger.InfoFormat("Successfully fetched {Count} employees.", employees.Count);

                return employees;

            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while fetching employees.", ex);
                throw new Exception("An error occurred while retrieving employees.", ex);
            }


        }

        internal async Task<CRUDEmployeeResponse> AddEmployee(AddEmployeeRequest request, ClaimsPrincipal user)
        {
            _logger.Info("Creating a new employee...");
            try
            {
                var managerIdClaim = user.FindFirst(JwtRegisteredClaimNames.Jti);
                //if (managerIdClaim == null)
                //{
                //    return Unauthorized(new { message = "Unauthorized access." });
                //}
                Guid managerId = Guid.Parse(managerIdClaim.Value);

                Employee newEmployee = new Employee(request.Email, request.FullName, request.Password, managerId);
              
                var data = _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();
                _logger.InfoFormat("Employee created successfully with ID: {Id}.", newEmployee.Id);

                return new CRUDEmployeeResponse() { EmployeeData = newEmployee };
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while creating a new employee.", ex);
                throw new Exception("An error occurred while creating the employee.", ex);
            }
        }

        internal async Task<bool> DeleteEmployee(Guid id)
        {
            _logger.InfoFormat("Deleting employee with ID: {Id}.", id);
            try
            {
                var employee = await _context.Employees.FindAsync(id);

                if (employee == null)
                {
                    _logger.WarnFormat("Employee with ID {Id} not found for deletion.", id);
                    return false;
                }
                var data = _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                _logger.InfoFormat("Employee with ID: {Id} deleted successfully.", id);
                return true;
            }



            catch (Exception ex)
            {
                _logger.ErrorFormat("Error occurred while deleting employee with ID: {Id}.", id, ex);
                throw new Exception("An error occurred while deleting the employee.", ex);
            }
        }

        internal async Task<Employee> EditEmployee(EmployeeBase employee)
        {
            var exsitingEmployee = await _context.Employees.FirstOrDefaultAsync(employee => employee.Id == employee.Id);

            if (exsitingEmployee == null)
            {
                throw new Exception("Employee not found");
            }


            exsitingEmployee.Email = employee.Email;
            exsitingEmployee.FullName = employee.FullName;
            exsitingEmployee.Password = employee.Password;

            var data = _context.Employees.Update(exsitingEmployee);
            await _context.SaveChangesAsync();
            return exsitingEmployee;
        }
    }
}
