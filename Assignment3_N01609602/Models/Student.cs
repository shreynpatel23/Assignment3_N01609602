using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment3_N01609602.Models
{
    public class Student
    {
        // id of the student
        public int id { get; set; }     
        // first name of the student
        public string firstName { get; set; }
        // last name of the student
        public string lastName { get; set; }
        // student number
        public string studentNumber { get; set; }
        // date on which the student is enrolled
        public string enrolDate { get; set; }
    }
}