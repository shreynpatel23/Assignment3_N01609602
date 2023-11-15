using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment3_N01609602.Models
{
    public class Teacher
    {
        public int id { get; set; } 
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string employeeNumber { get; set; }
        public string hireDate { get; set; }
        public decimal salary { get; set; }
    }
}