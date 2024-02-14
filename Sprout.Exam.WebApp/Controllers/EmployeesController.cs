using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Interface;
using Sprout.Exam.WebApp.Models;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeInterface _employeeRepository;

        public EmployeesController(IEmployeeInterface employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployee()
         {
            var employees  = await _employeeRepository.GetAllEmployee();
            //var result = await Task.FromResult(StaticEmployees.ResultList);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] int id)
        {
             var employee = await _employeeRepository.GetEmployeeById(id);
            //var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));
            return Ok(employee);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee newEmployee, [FromRoute] int id)
        {
            await _employeeRepository.UpdateEmployee(id, newEmployee);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody]Employee newEmployee)
        {

            var id = await _employeeRepository.AddEmployee(newEmployee);
            return CreatedAtAction(nameof(GetEmployeeById),
                new
                {
                    id = id,
                    controller = "employees"
                }, id
                );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            await _employeeRepository.DeleteEmployee(id);
            return Ok();
        }

        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate([FromRoute] int id, [FromBody] CalculateDto newEmployee)
        {
            var result = await _employeeRepository.Calculate(id, (int)newEmployee.WorkedDays, (int)newEmployee.AbsentDays);
            return Ok(result);

    }

    }
}
