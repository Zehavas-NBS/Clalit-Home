using EmployeeManagerAPI.Models;
using EmployeeManagerAPI.Models.Requests;
using EmployeeManagerAPI.Services;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EmployeeManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeesService _employeesService;
        private static readonly ILog log = LogManager.GetLogger(typeof(EmployeesController));


        public EmployeesController(EmployeesService employeesService)
        {
            _employeesService = employeesService;

        }

        [HttpPost("getEmployeesByManagerId")]
        [Authorize]
        public async Task<IActionResult> GetEmployeesByManagerId()
        {
            try
            {
                log.Info("Fetching all employees...");

                var response = await _employeesService.GetEmployeesByManagerId(User);
                log.InfoFormat("Successfully fetched {Count} employees.", response.Count);

                return Ok(response);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while fetching employees.", ex);
                return StatusCode(500, "An error occurred while retrieving employees.");
            }
        }

        [HttpPost("add")]
        [Authorize]

        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeRequest request)
        {
            try
            {
                log.Info($"Adding new employee: {request.FullName}");

                var response = await _employeesService.AddEmployee(request, User);
                log.InfoFormat("Employee created successfully with ID: {Id}", response.EmployeeData.Id);

                //if (response == null)
                //    return BadRequest("Employee data is invalid.");

                return Ok(new { message = "Employee added successfully.", response });

            }
            catch (Exception ex)
            {

                log.Error("Error adding new employee", ex);
                return StatusCode(500, "An error occurred while creating the employee.");
            }

        }

        // עדכון עובד קיים
        [HttpPut("update")]
        [Authorize]

        public async Task<IActionResult> UpdateEmployee([FromBody] AddEmployeeRequest updatedEmployee)
        {
            log.InfoFormat("Updating employee with ID: {Id}", updatedEmployee.Email);

            try
            {


                var response = await _employeesService.EditEmployee(updatedEmployee);
                if (response == null)
                {
                    log.WarnFormat("Employee with ID {Id} not found for update.", updatedEmployee.Email);
                    return NotFound("Employee not found.");
                }
                log.InfoFormat("Employee with ID: {Id} updated successfully.", updatedEmployee.Email);


                return Ok(new { message = "Employee updated successfully.", response });
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error occurred while updating employee with ID: {Id}", updatedEmployee.Email, ex);
                return StatusCode(500, "An error occurred while updating the employee.");
            }
        }

        // מחיקת עובד לפי ID
        [HttpDelete("delete/{id}")]
        [Authorize]

        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            log.InfoFormat("Deleting employee with ID: {Id}", id);
            try
            {
                var result = await _employeesService.DeleteEmployee(id);


                if (!result)
                {
                    log.WarnFormat("Employee with ID {Id} not found for deletion.", id);
                    return NotFound("Employee not found.");
                }
                log.InfoFormat("Employee with ID: {Id} deleted successfully.", id);
                return Ok("Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error occurred while deleting employee with ID: {Id}", id, ex);
                return StatusCode(500, "An error occurred while deleting the employee.");
            }
        }
    }
}
