using EmployeeManagerAPI.Data;
using EmployeeManagerAPI.Models;
using EmployeeManagerAPI.Models.Requests;
using EmployeeManagerAPI.Models.Responses;
using EmployeeManagerAPI.Validations;
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

        // Helper method to extract ManagerId from Claims
        private Guid GetManagerIdFromClaims(ClaimsPrincipal user)
        {
            var managerIdClaim = user.FindFirst(JwtRegisteredClaimNames.Jti);
            if (managerIdClaim == null || string.IsNullOrEmpty(managerIdClaim.Value))
            {
                throw new InvalidOperationException("Manager ID claim is missing or invalid.");
            }

            return Guid.Parse(managerIdClaim.Value);
        }

        public async Task<ICollection<GetEmployeesResponse>> GetEmployeesByManagerId(ClaimsPrincipal user)
        {
            _logger.Info("Fetching all employees from the database.");

            try
            {
                // Extract ManagerId from Claims
                var managerId = GetManagerIdFromClaims(user);

                // Fetch employees directly based on ManagerId
                var employees = await _context.Employees
                    .Where(emp => emp.ManagerId == managerId)
                    .Select(emp => new GetEmployeesResponse
                    {
                        Email = emp.Email,
                        FullName = emp.FullName,
                        Id = emp.Id,
                        Password = emp.Password
                    })
                    .ToListAsync();
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
            _logger.Info("Initiating the creation of a new employee.");

            try
            {
                // Extract ManagerId from Claims
                var managerId = GetManagerIdFromClaims(user);

                // Validate email format
                if (string.IsNullOrWhiteSpace(request.Email) || !ValidationHelper.IsValidEmail(request.Email))
                {
                    _logger.Warn($"Invalid email format: {request.Email}");
                    throw new ArgumentException(ErrorMessages.InvalidEmail);
                }


                // Validate full name
                if (string.IsNullOrWhiteSpace(request.FullName) || request.FullName.Length < 2)
                {
                    _logger.Warn($"Invalid full name provided: {request.FullName}");
                    throw new ArgumentException(ErrorMessages.InvalidFullName);

                }

                // Create new Employee instance
                var newEmployee = new Employee(request.Email, request.FullName, request.Password, managerId);

                // Add to database
                _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();

                _logger.InfoFormat("Employee successfully created with ID: {Id}.", newEmployee.Id);

                return new CRUDEmployeeResponse { EmployeeData = newEmployee };
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error("Failed to retrieve manager ID from user claims.", ex);
                throw new UnauthorizedAccessException("Unable to identify the manager. Unauthorized request.", ex);
            }
            catch (Exception ex)
            {
                _logger.Error("An unexpected error occurred while creating the employee.", ex);
                throw new Exception("An error occurred while creating the employee. Please try again later.", ex);
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

        internal async Task<Employee> EditEmployee(AddEmployeeRequest employee)
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
