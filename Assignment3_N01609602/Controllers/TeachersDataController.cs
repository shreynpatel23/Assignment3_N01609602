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
    public class TeachersDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of teachers
        /// </summary>
        /// <example>GET api/fetchAllTeachers</example>
        /// <returns>
        /// A list of teachers objects.
        /// </returns>
        [HttpGet]
        [Route("api/fetchAllTeachers/{searchKey?}")]
        public IEnumerable<Teacher> FetchAllTeachers(string searchKey)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + searchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of teachers
            List<Teacher> teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int teacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string teacherFirstname = ResultSet["teacherfname"].ToString();
                string teacherLastname = ResultSet["teacherlname"].ToString();

                Teacher newTeacher = new Teacher();
                newTeacher.id = teacherId;
                newTeacher.firstName = teacherFirstname;
                newTeacher.lastName = teacherLastname;

                //Add the new teacher to the List
                teachers.Add(newTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teachers
            return teachers;
        }

        /// <summary>
        /// Returns details of the teacher with a given id
        /// </summary>
        /// <param name="id">the id of the teacher to fetch details</param>
        /// <example>GET api/fetchTeacherDetails/2</example>
        /// <returns>
        /// A Single teacher details
        /// </returns>
        [HttpGet]
        [Route("api/fetchTeacherDetails/{id}")]
        public Teacher FetchTeacherDetails(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where teacherid = @id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty teacher object
            Teacher teacher = new Teacher();

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                string teacherFirstname = ResultSet["teacherfname"].ToString();
                string teacherLastname = ResultSet["teacherlname"].ToString();
                string employeeNumber = ResultSet["employeenumber"].ToString();
                string hireDate = Convert.ToDateTime(ResultSet["hiredate"]).ToString("dd/MM/yyyy");
                decimal salary = Convert.ToDecimal(ResultSet["salary"]);

                teacher.firstName = teacherFirstname;
                teacher.lastName = teacherLastname;
                teacher.employeeNumber = employeeNumber;
                teacher.hireDate = hireDate;
                teacher.salary = salary;
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the teacher data
            return teacher;
        }
    }
}
