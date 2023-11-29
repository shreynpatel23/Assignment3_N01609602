using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment3_N01609602.Models
{
    public class Classes
    {
        // id of the course
        public int id { get; set; }
        // name of the course
        public string className { get; set; }
        // code of the course
        public string classCode {  get; set; }
        // start date of the course
        public string startDate { get; set; }
        // end date of the course
        public string endDate { get; set; }
        // teacher first name teaching that course
        public string teacherFirstName { get; set; }
        // teacher last name teaching the course
        public string teacherLastName { get; set;}
      
    }
}