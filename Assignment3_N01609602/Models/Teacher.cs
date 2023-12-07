using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment3_N01609602.Models
{
    public class Teacher
    {
        // id of the teacher
        public int id { get; set; } 
        // first name of the teacher
        public string firstName { get; set; }
        // last name of the teacher
        public string lastName { get; set; }
        // employee number of the teacher 
        public string employeeNumber { get; set; }
        // hire date of the teacher
        public string hireDate { get; set; }
        // salary of the teacher
        public decimal salary { get; set; }

        public bool IsValid()
        {
            bool valid = true;

            if (firstName == null || lastName == null || employeeNumber == null || salary <=0 )
            {
                // check if all the fields are present or not
                valid = false;
            }

            return valid;
        }
    }
}