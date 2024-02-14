using Sprout.Exam.WebApp.Data;
using Sprout.Exam.WebApp.Interface;
using Sprout.Exam.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Sprout.Exam.WebApp.Repository
{
    public class EmployeeRepository : IEmployeeInterface
    {

        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Employee>> GetAllEmployee()
        {
            var employee = await _context.Employees.Select(x => new Employee()
            {
                Id = x.Id,
                EmployeeTypeId = x.EmployeeTypeId,
                FullName = x.FullName,
                Birthdate = x.Birthdate,
                Tin = x.Tin,

            }).ToListAsync();

            return employee;
        }
        public async Task<Employee> GetEmployeeById(int employeeId)
        {
            var employee = await _context.Employees.Where(x => x.Id == employeeId).Select(x => new Employee()
            {
                Id = x.Id,
                EmployeeTypeId = x.EmployeeTypeId,
                FullName = x.FullName,
                Birthdate = x.Birthdate,
                Tin = x.Tin,

            }).FirstOrDefaultAsync();

            return employee;
        }

        public async Task<int> AddEmployee(Employee newEmployee)
        {
            var employee = new Employee()
            {
                EmployeeTypeId = newEmployee.EmployeeTypeId,
                FullName = newEmployee.FullName,
                Birthdate = newEmployee.Birthdate,
                Tin = newEmployee.Tin

            };

            var employeeType = await _context.EmployeeTypes.FindAsync(newEmployee.EmployeeTypeId);
            if (employeeType != null)
            {
                employee.EmployeeTypeId = employeeType.Id;
            }
            else
            {
                employee.EmployeeTypeId = 2;
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return employee.Id;
        }

        public async Task UpdateEmployee(int employeeId, Employee newEmployee)
        {
            var employee = new Employee()
            {

                Id = newEmployee.Id,
                EmployeeTypeId = newEmployee.EmployeeTypeId,
                FullName = newEmployee.FullName,
                Birthdate = newEmployee.Birthdate,
                Tin = newEmployee.Tin
            };


               _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
             
        }

        public async Task DeleteEmployee(int employeeId)
        {
            var employee = new Employee()
            {
                Id = employeeId
            };

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }


        public async Task<decimal> Calculate(int employeeId, decimal workedDays, decimal absentDays)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);

            if (employee == null)
                throw new ArgumentException("Employee not found");

            decimal basicSalary;
            decimal calculatedSalary = 0;
           
            if (employee.EmployeeTypeId == 1) 
            {
                basicSalary = 20000;
                decimal deduction = basicSalary / 22;
                decimal tax = basicSalary * 0.12m;
                decimal netSalary = basicSalary - deduction * absentDays - tax;
                calculatedSalary = Math.Max(netSalary, 0); 
            }
            else if (employee.EmployeeTypeId == 2)
            {
                basicSalary = 500;
                decimal totalSalary = basicSalary * workedDays;
                calculatedSalary = totalSalary;
            }
            else
            {
                throw new ArgumentException("Invalid employee type");
            }

            calculatedSalary = Math.Round(calculatedSalary, 2);

            return calculatedSalary;

        }


    }
}
