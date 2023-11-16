using Assignment3_N01609602.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Assignment3_N01609602.Controllers
{
    public class StudentsDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of students
        /// </summary>
        /// <example>GET api/fetchAllStudents</example>
        /// <returns>
        /// A list of students objects.
        /// </returns>
        [HttpGet]
        [Route("api/fetchAllStudents")]
        public IEnumerable<Student> FetchAllStudents()
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from students";

            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of students
            List<Student> students = new List<Student> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int studentId = Convert.ToInt32(ResultSet["studentid"]);
                string studentFirstName = ResultSet["studentfname"].ToString();
                string studentLastName = ResultSet["studentlname"].ToString();

                Student newStudent = new Student();
                newStudent.id = studentId;
                newStudent.firstName = studentFirstName;
                newStudent.lastName = studentLastName;

                //Add the new student to the List
                students.Add(newStudent);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of students
            return students;
        }

        /// <summary>
        /// Returns details of the student with a given id
        /// </summary>
        /// <param name="id">the id of the student to fetch details</param>
        /// <example>GET api/fetchStudentDetails/2</example>
        /// <returns>
        /// A Single student details
        /// </returns>
        [HttpGet]
        [Route("api/fetchStudentDetails/{id}")]
        public Student FetchStudentDetails(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from students where studentid = @id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty student object
            Student student = new Student();

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int studentId = Convert.ToInt32(ResultSet["studentid"]);
                string studentFirstname = ResultSet["studentfname"].ToString();
                string studentLastname = ResultSet["studentlname"].ToString();
                string studentNumber = ResultSet["studentnumber"].ToString();
                string studentEnrolDate = ResultSet["enroldate"].ToString();

                student.id = studentId;
                student.firstName = studentFirstname;
                student.lastName = studentLastname;
                student.studentNumber= studentNumber;
                student.enrolDate = studentEnrolDate;
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the teacher data
            return student;
        }

        /// <summary>
        /// Returns all the classes that the student is enrolled
        /// </summary>
        /// <param name="studentId">The student id for which we want to fetch details</param>
        /// <example>GET api/fetchAllClassesOfStudent/1</example>
        /// <returns>
        /// List of classes the student is enrolled in
        /// </returns>
        [HttpGet]
        [Route("api/fetchAllClassesOfStudent/{studentId}")]
        public List<Classes> fetchAllClassesOfStudent(int studentId)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            // FOR THIS API I HAVE CREATED A VIEW IN MY DATABASE CALLED GET_CLASSES_OF_STUDENT
            // IT WILL RETURN THE CLASS INFORMATION AS WELL AS THE TEACHER TEACHING THE CLASS
            cmd.CommandText = "Select * from get_classes_of_students where studentid = @studentid";

            cmd.Parameters.AddWithValue("@studentid", studentId);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty classes list
            List<Classes> classes = new List<Classes>();   

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                string className = ResultSet["classname"].ToString();
                string classCode = ResultSet["classcode"].ToString();
                string startDate = ResultSet["startdate"].ToString();
                string endDate = ResultSet["finishdate"].ToString();
                string teacherFirstName = ResultSet["teacherfname"].ToString();
                string teacherLastName = ResultSet["teacherlname"].ToString();

                //Create an new class object
                Classes newClass = new Classes();

                //Update the fetched data and put it in the new object
                newClass.className = className;
                newClass.classCode = classCode;
                newClass.startDate = startDate;
                newClass.endDate = endDate;
                newClass.teacherFirstName = teacherFirstName;
                newClass.teacherLastName= teacherLastName;


                //add the newClass object in the classes list
                classes.Add(newClass);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the classes list data
            return classes;

        }
    }
}
