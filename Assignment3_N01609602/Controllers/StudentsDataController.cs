﻿using Assignment3_N01609602.Models;
using Google.Protobuf;
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
    public class StudentsDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of students
        /// </summary>
        /// <param name="searchKey">the term that the user want to serve</param>
        /// <example>GET api/fetchAllStudents</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/fetchAllStudents"
        /// GET: curl "http://localhost:50860/api/fetchAllStudents"
        /// GET: curl "http://localhost:50860/api/fetchAllStudents"
        /// </example>
        /// <returns>
        /// A list of students objects.
        /// </returns>
        [HttpGet]
        [Route("api/fetchAllStudents/{searchKey?}")]
        public IEnumerable<Student> FetchAllStudents(string searchKey)
        {
            // create an instance
            MySqlConnection Conn = School.AccessDatabase();

            // create an empty list of students
            List<Student> students = new List<Student> { };

            try
            {
                // open the connection
                Conn.Open();

                // create a command
                MySqlCommand cmd = Conn.CreateCommand();

                // query
                cmd.CommandText = "Select * from students where lower(studentfname) like lower(@key) or lower(studentlname) like lower(@key) or lower(concat(studentfname, ' ', studentlname)) like lower(@key)";

                // parameterise the query
                cmd.Parameters.AddWithValue("@key", "%" + searchKey + "%");
                cmd.Prepare();

                // gather Result Set
                MySqlDataReader ResultSet = cmd.ExecuteReader();

                // loop Through Each Result Set
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

                    // add the new student to the List
                    students.Add(newStudent);
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

            // return the final list of students
            return students;
        }

        /// <summary>
        /// Returns details of the student with a given id
        /// </summary>
        /// <param name="id">the id of the student to fetch details</param>
        /// <example>GET api/fetchStudentDetails/2</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/fetchStudentDetails/1"
        /// GET: curl "http://localhost:50860/api/fetchStudentDetails/2"
        /// GET: curl "http://localhost:50860/api/fetchStudentDetails/3"
        /// </example>
        /// <returns>
        /// A Single student details
        /// </returns>
        [HttpGet]
        [Route("api/fetchStudentDetails/{id}")]
        public Student FetchStudentDetails(int id)
        {
            // create an instance
            MySqlConnection Conn = School.AccessDatabase();

            // create an empty student object
            Student student = new Student();

            try
            {

                // open the connection
                Conn.Open();

                // create a command
                MySqlCommand cmd = Conn.CreateCommand();

                // query
                cmd.CommandText = "Select * from students where studentid = @id";

                // parameterise the query
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                // gather Result Set
                MySqlDataReader ResultSet = cmd.ExecuteReader();

                // loop Through Each Row 
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int studentId = Convert.ToInt32(ResultSet["studentid"]);
                    string studentFirstname = ResultSet["studentfname"].ToString();
                    string studentLastname = ResultSet["studentlname"].ToString();
                    string studentNumber = ResultSet["studentnumber"].ToString();
                    string studentEnrolDate = Convert.ToDateTime(ResultSet["enroldate"]).ToString("dd/MM/yyyy");

                    student.id = studentId;
                    student.firstName = studentFirstname;
                    student.lastName = studentLastname;
                    student.studentNumber = studentNumber;
                    student.enrolDate = studentEnrolDate;
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
                // close the connection between the MySQL Database and the WebServer
                Conn.Close();
            }

            // return the teacher data
            return student;
        }

        /// <summary>
        /// Returns all the classes that the student is enrolled
        /// </summary>
        /// <param name="studentId">The student id for which we want to fetch details</param>
        /// <example>GET api/fetchAllCoursesOfStudent/1</example>
        /// <example>
        /// GET: curl "http://localhost:50860/api/fetchAllCoursesOfStudent/1"
        /// GET: curl "http://localhost:50860/api/fetchAllCoursesOfStudent/2"
        /// GET: curl "http://localhost:50860/api/fetchAllCoursesOfStudent/3"
        /// </example>
        /// <returns>
        /// List of classes the student is enrolled in
        /// </returns>
        [HttpGet]
        [Route("api/fetchAllCoursesOfStudent/{studentId}")]
        public List<Course> fetchAllCoursesOfStudent(int studentId)
        {
            // create an instance
            MySqlConnection Conn = School.AccessDatabase();

            // create an empty courses list
            List<Course> courses = new List<Course>();

            try
            {
                // open the connection
                Conn.Open();

                // create a command
                MySqlCommand cmd = Conn.CreateCommand();

                // query
                // FOR THIS API I HAVE CREATED A VIEW IN MY DATABASE CALLED GET_CLASSES_OF_STUDENT
                // IT WILL RETURN THE CLASS INFORMATION AS WELL AS THE TEACHER TEACHING THE CLASS
                cmd.CommandText = "Select * from get_classes_of_students where studentid = @studentid";

                // parameterise the query
                cmd.Parameters.AddWithValue("@studentid", studentId);
                cmd.Prepare();

                // gather Result Set of Query into a variable
                MySqlDataReader ResultSet = cmd.ExecuteReader();

                // loop Through Each Row the Result Set
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    string className = ResultSet["classname"].ToString();
                    string classCode = ResultSet["classcode"].ToString();
                    string startDate = Convert.ToDateTime(ResultSet["startdate"]).ToString("dd/MM/yyyy");
                    string endDate = Convert.ToDateTime(ResultSet["finishdate"]).ToString("dd/MM/yyyy");
                    string teacherFirstName = ResultSet["teacherfname"].ToString();
                    string teacherLastName = ResultSet["teacherlname"].ToString();

                    // create an new course object
                    Course newCourse = new Course();

                    // update the fetched data and put it in the new object
                    newCourse.className = className;
                    newCourse.classCode = classCode;
                    newCourse.startDate = startDate;
                    newCourse.endDate = endDate;
                    newCourse.teacherFirstName = teacherFirstName;
                    newCourse.teacherLastName = teacherLastName;


                    // add the new course object in the courses list
                    courses.Add(newCourse);
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
                // close the connection between the MySQL Database and the WebServer
                Conn.Close();
            }

            // return the classes list data
            return courses;

        }
    }
}
