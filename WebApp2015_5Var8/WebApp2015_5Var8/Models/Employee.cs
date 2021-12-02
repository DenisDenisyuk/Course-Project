using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp2015_5Var8.Models
{
    public class Employee
    {
        // описывает структуру записи о сотруднике
        public int ID { get; set; }
        public string Lastname { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AcademicDegree { get; set; }
        public string Position { get; set; }
        public double Salary { get; set; }

        public List<Employee> EmployeeList { get; set; } // список всех сотрудников
    }
}