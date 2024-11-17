using EmployeeManagerAPI.Models;
using EmployeeManagerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeesService _employeesService;

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
                var response = await _employeesService.GetEmployeesByManagerId(User);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddEmployee([FromBody] BaseEntity employee)
        {
            var response = await _employeesService.AddEmployee(employee ,User);
            if (response == null)
                return BadRequest("Employee data is invalid.");

            return Ok(new { message = "Employee added successfully.", response });
        }

        // עדכון עובד קיים
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] BaseEntity updatedEmployee)
        {
            var response = await _employeesService.EditEmployee(updatedEmployee);
            //var employee = await _context.Employees.FindAsync(id);

            //if (employee == null)
            //    return NotFound(new { message = "Employee not found." });

            //// עדכון שדות העובד
            //employee.Email = updatedEmployee.Email;
            //employee.FullName = updatedEmployee.FullName;
            //employee.Password = updatedEmployee.Password; // הנח כי הסיסמה כבר מוצפנת
            //employee.CreatedAt = updatedEmployee.CreatedAt;

            //_context.Employees.Update(employee);
            //await _context.SaveChangesAsync();

            return Ok(new { message = "Employee updated successfully.", response });
        }

        // מחיקת עובד לפי ID
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var response = await _employeesService.DeleteEmployee(id);


            if (response == null)
                return NotFound(new { message = "Employee not found." });


            return Ok(new { message = "Employee deleted successfully." });
        }
    }
}
