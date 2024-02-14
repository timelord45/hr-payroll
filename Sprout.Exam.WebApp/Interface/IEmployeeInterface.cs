using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Interface
{
    public interface IEmployeeInterface
    {
        Task<List<Employee>> GetAllEmployee();
        Task<int> AddEmployee(Employee newEmployee);
        Task<Employee> GetEmployeeById(int employeeId);
        Task UpdateEmployee(int employeeId, Employee newEmployee);

        Task DeleteEmployee(int employeeId);
        Task<decimal> Calculate(int employeeId, decimal workedDays, decimal absentDays);
    }
}
