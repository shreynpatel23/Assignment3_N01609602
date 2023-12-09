using Assignment3_N01609602.Models;
using Microsoft.AspNetCore.Cors;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// <param name="searchKey">the term that the user want to serve</param>
        /// <example>GET api/fetchAllTeachers</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/fetchAllTeachers"
        /// GET: curl "http://localhost:50860/api/fetchAllTeachers"
        /// GET: curl "http://localhost:50860/api/fetchAllTeachers"
        /// </example>
        /// <returns>
        /// A list of teachers objects.
        /// </returns>
        [HttpGet]
        [Route("api/fetchAllTeachers/{searchKey?}")]
        public IEnumerable<Teacher> FetchAllTeachers(string searchKey)
        {
            // create an empty list of teachers
            List<Teacher> teachers = new List<Teacher> { };

            // create an instance
            MySqlConnection Conn = School.AccessDatabase();

            try
            {
                // open the connection
                Conn.Open();

                // create a command
                MySqlCommand cmd = Conn.CreateCommand();

                // query
                cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";

                cmd.Parameters.AddWithValue("@key", "%" + searchKey + "%");
                cmd.Prepare();

                // extract data into resultSet
                MySqlDataReader resultSet = cmd.ExecuteReader();

                // loop Through Each Result Set
                while (resultSet.Read())
                {
                    // access Column information by the DB column name as an index
                    int teacherId = Convert.ToInt32(resultSet["teacherid"]);
                    string teacherFirstname = resultSet["teacherfname"].ToString();
                    string teacherLastname = resultSet["teacherlname"].ToString();

                    Teacher newTeacher = new Teacher();
                    newTeacher.id = teacherId;
                    newTeacher.firstName = teacherFirstname;
                    newTeacher.lastName = teacherLastname;

                    // add the new teacher to the List
                    teachers.Add(newTeacher);
                }

            }
            catch (MySqlException ex)
            {
                // database issue
                throw new ApplicationException("Someething wrong with the database!.", ex);
            }
            catch (Exception ex)
            {
                // server issue
                throw new ApplicationException("Someething wrong with the server!.", ex);
            }
            finally
            {
                // close the connection
                Conn.Close();
            }
            
            // return the list of teachers
            return teachers;
        }

        /// <summary>
        /// Returns details of the teacher with a given id
        /// </summary>
        /// <param name="id">the id of the teacher to fetch details</param>
        /// <example>GET api/fetchTeacherDetails/2</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/fetchTeacherDetails/1"
        /// GET: curl "http://localhost:50860/api/fetchTeacherDetails/4"
        /// GET: curl "http://localhost:50860/api/fetchTeacherDetails/8"
        /// </example>
        /// <returns>
        /// A Single teacher details
        /// </returns>
        [HttpGet]
        [Route("api/fetchTeacherDetails/{id}")]
        public Teacher FetchTeacherDetails(int id)
        {
            // create an instance 
            MySqlConnection Conn = School.AccessDatabase();

            // create an empty teacher object
            Teacher teacher = new Teacher();

            try
            {
                // open the connection 
                Conn.Open();

                // create a command
                MySqlCommand cmd = Conn.CreateCommand();

                // query
                cmd.CommandText = "Select * from teachers where teacherid = @id";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                // extract data into resultSet
                MySqlDataReader resultSet = cmd.ExecuteReader();

                // loop Through Each data
                while (resultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int teacherId = Convert.ToInt32(resultSet["teacherid"]);
                    string teacherFirstname = resultSet["teacherfname"].ToString();
                    string teacherLastname = resultSet["teacherlname"].ToString();
                    string employeeNumber = resultSet["employeenumber"].ToString();
                    string hireDate = Convert.ToDateTime(resultSet["hiredate"]).ToString("dd/MM/yyyy");
                    decimal salary = Convert.ToDecimal(resultSet["salary"]);

                    teacher.id = teacherId;
                    teacher.firstName = teacherFirstname;
                    teacher.lastName = teacherLastname;
                    teacher.employeeNumber = employeeNumber;
                    teacher.hireDate = hireDate;
                    teacher.salary = salary;
                }
            }
            catch (MySqlException ex)
            {
                // database issue
                Debug.Write(ex);
                throw new ApplicationException("Someething wrong with the database!.", ex);
            }
            catch (Exception ex)
            {
                // server issue
                Debug.Write(ex);
                throw new ApplicationException("Someething wrong with the server!.", ex);
            }

            finally
            {
                // close the connection
                Conn.Close();
            }

            // return the teacher data
            return teacher;
        }

        /// <summary>
        /// Returns all the classes that the teacher is teaching
        /// </summary>
        /// <param name="teacherId">The teacher id for which we want to fetch details</param>
        /// <example>GET api/fetchAllCoursesOfTeacher/1</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/fetchAllCoursesOfTeacher/2"
        /// GET: curl "http://localhost:50860/api/fetchAllCoursesOfTeacher/3"
        /// GET: curl "http://localhost:50860/api/fetchAllCoursesOfTeacher/4"
        /// </example>
        /// <returns>
        /// List of classes that the teacher is teaching
        /// </returns>
        [HttpGet]
        [Route("api/fetchAllCoursesOfTeacher/{teacherid}")]
        public IEnumerable<Course> fetchAllCoursesOfTeacher(int teacherId)
        {
            // create an instance 
            MySqlConnection Conn = School.AccessDatabase();

            // create an empty list of classes
            List<Course> classes = new List<Course> { };

            try
            {
                // open the connection
                Conn.Open();

                // create a command
                MySqlCommand cmd = Conn.CreateCommand();

                // query
                // FOR THIS QUERY ALSO I HAVE CREATED A VIEW CALLED GET_ALL_SUBJECTS_OF_TEACHER
                cmd.CommandText = "Select * from get_all_teachers_classes where teacherid = @teacherid";

                cmd.Parameters.AddWithValue("@teacherid", teacherId);
                cmd.Prepare();

                // gather Result Set
                MySqlDataReader resultSet = cmd.ExecuteReader();

                // loop Through Each Result Set
                while (resultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    string className = resultSet["classname"].ToString();
                    string classStartDate = Convert.ToDateTime(resultSet["startdate"]).ToString("dd/MM/yyyy");
                    string classEndDate = Convert.ToDateTime(resultSet["finishdate"]).ToString("dd/MM/yyyy");

                    Course newClass = new Course();
                    newClass.className = className;
                    newClass.startDate = classStartDate;
                    newClass.endDate = classEndDate;

                    // add the new class to the List
                    classes.Add(newClass);
                }
            }
            catch (MySqlException ex)
            {
                // database issue
                throw new ApplicationException("Someething wrong with the database!.", ex);
            }
            catch (Exception ex)
            {
                // server issue
                throw new ApplicationException("Someething wrong with the server!.", ex);
            }
            finally
            {
                // close the connection
                Conn.Close();
            }

            // return the of classes
            return classes;
        }


        /// <summary>
        /// Adds a teacher to the MySQL Database.
        /// </summary>
        /// <param name="newTeacher">the new teacher object that is created with the form fields</param>
        /// <example>
        /// POST api/addTeacher
        /// Request body
        /// {
        ///	"firstName":"John",
        ///	"lastName":"Doe",
        ///	"employeeNumber":"J123",
        ///	"salary":"10000"
        /// }
        /// </example>
        /// <example>
        /// POST: curl "http://localhost:50860/api/addTeacher" -d @addTeacher.json -H "Content-type: application/json"
        /// </example>
        [HttpPost]
        [Route("api/addTeacher")]
        public void AddTeacher([FromBody] Teacher newTeacher)
        {

            // create an instance 
            MySqlConnection Conn = School.AccessDatabase();

            try
            {
                // check if all the required feilds are present or not
                if (!newTeacher.IsValid())
                {
                    throw new ApplicationException("All Four fieldd, First Name, Last Name, Employee Number And Salary are required!");
                }

                // open the connection
                Conn.Open();

                // create a command
                MySqlCommand cmd = Conn.CreateCommand();

                // query
                // FOR THIS QUERY ALSO I HAVE CREATED A PROCEDURE CALLED ADD_TEACHER
                // this procedure will take four paramerters(firstname, lastname, salary, employeeNumber);
                // it will insert into the teachers table
                cmd.CommandText = "CALL AddTeacher(@firstName, @lastName, @salary, @employeeNumber);";

                // parameterise the variables to stop sql injection
                cmd.Parameters.AddWithValue("@firstName", newTeacher.firstName);
                cmd.Parameters.AddWithValue("@lastName", newTeacher.lastName);
                cmd.Parameters.AddWithValue("@employeeNumber", newTeacher.employeeNumber);
                cmd.Parameters.AddWithValue("@salary", newTeacher.salary);
                cmd.Prepare();

                // execute the query
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                // database issue
                throw new ApplicationException("Someething wrong with the database!.", ex);
            }
            catch (Exception ex)
            {
                // server issue
                throw new ApplicationException("Someething wrong with the server!.", ex);
            }

            finally { 
                // close the connection
                Conn.Close(); 
            }


        }

        /// <summary>
        /// Deletes a teacher from the teacher list
        /// </summary>
        /// <param name="teacherId">The teacher id which we want to remove.</param>
        /// <example>POST api/deleteTeacher/3</example>
        /// <example>
        /// POST: curl "http://localhost:50860/api/deleteTeacher/16" -d "" -H "Content-type: application/json"
        /// </example>
        [HttpPost]
        [Route("api/deleteTeacher/{teacherId}")]
        public void DeleteTeacher(string teacherId)
        {
            // create an instance 
            MySqlConnection Conn = School.AccessDatabase();

            try
            {
                // check if the id is present or not
                if (teacherId == "")
                {
                    throw new ApplicationException("The Id of the teacher is required!");
                }
                // open the connection
                Conn.Open();

                // create a command
                MySqlCommand cmd = Conn.CreateCommand();

                // query
                cmd.CommandText = "Delete from teachers where teacherid=@teacherId";
                cmd.Parameters.AddWithValue("@teacherId", teacherId);
                cmd.Prepare();

                // execute the query
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                // database issue
                throw new ApplicationException("Someething wrong with the database!.", ex);
            }
            catch (Exception ex)
            {
                // server issue
                throw new ApplicationException("Someething wrong with the server!.", ex);
            }

            finally
            {
                // close the connection
                Conn.Close();
            }

        }

        /// <summary>
        /// Updates the teacher info for the given id
        /// </summary>
        /// <param name="teacherId">The id for which the teacher info needs to be changed</param>
        /// <param name="newTeacher">The updated teacher object</param>
        /// <example>
        /// POST api/updateTeacher/2
        /// Form Data, request body
        /// {
        ///	"firstName":"John updated",
        ///	"lastName":"Doe updated",
        ///	"employeeNumber":"J124",
        ///	"salary":"1000"
        /// }
        /// </example>
        /// <example>
        /// METHOD 2 --> POST : curl "http://localhost:50860/api/updateTeacher/2" -d @updateTeachers.json -H "Content-type: application/json"
        /// METHOD 2 --> POST : curl "http://localhost:50860/api/updateTeacher/7" -d @updateTeacher.json -H "Content-type: application/json"
        /// </example>
        [HttpPost]
        [Route("api/updateTeacher/{teacherId}")]
        public void UpdateTeacher(string teacherId, [FromBody] Teacher newTeacher)
        {
            // create an instance 
            MySqlConnection Conn = School.AccessDatabase();

            try
            {
                if(teacherId == "")
                {
                    throw new ApplicationException("Teacher Id is required to be updated!");
                }
                // check if the id is present or not
                if (!newTeacher.IsValid())
                {
                    throw new ApplicationException("All Four fieldd, First Name, Last Name, Employee Number And Salary are required!");
                }
                // open the connection
                Conn.Open();

                // create a command
                MySqlCommand cmd = Conn.CreateCommand();

                // query
                cmd.CommandText = "UPDATE teachers SET teacherfname=@firstName, teacherlname=@lastName, employeenumber=@employeeNumber, salary=@salary WHERE teacherId=@teacherId";

                // parameterise the variables to stop sql injection
                cmd.Parameters.AddWithValue("@firstName", newTeacher.firstName);
                cmd.Parameters.AddWithValue("@lastName", newTeacher.lastName);
                cmd.Parameters.AddWithValue("@employeeNumber", newTeacher.employeeNumber);
                cmd.Parameters.AddWithValue("@salary", newTeacher.salary);
                cmd.Parameters.AddWithValue("@teacherId", teacherId);

                cmd.Prepare();

                cmd.ExecuteNonQuery();

                // execute the query
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                // database issue
                throw new ApplicationException("Someething wrong with the database!.", ex);
            }
            catch (Exception ex)
            {
                // server issue
                throw new ApplicationException("Someething wrong with the server!.", ex);
            }

            finally
            {
                // close the connection
                Conn.Close();
            }

        }
    }
}
