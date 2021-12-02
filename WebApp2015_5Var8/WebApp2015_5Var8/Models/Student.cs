using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp2015_5Var8.Models
{
    public class Student
    {
        // описывает структуру записи о студенте
        public int ID { get; set; }
        public string Lastname { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string CreditCardNumber { get; set; }
        public int EducationalForm { get; set; }
        public int BasisOfTraining { get; set; }
        
        public List<Student> StudentList { get; set; } // список всех студентов
    }

}