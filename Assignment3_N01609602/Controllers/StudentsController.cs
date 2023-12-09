using Assignment3_N01609602.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Assignment3_N01609602.Controllers
{
    public class StudentsController : Controller
    {
        // declare the controller here once and for all
        StudentsDataController controller = new StudentsDataController();

        // GET: Students/error?message={message}
        public ActionResult Error([FromUri] string message)
        {
            ViewBag.errorMessage = message;
            return View();
        }

        // GET: Students/List/{searchKey}
        public ActionResult List(string searchKey = "")
        {
            try
            {
                // get a list of Students.
                IEnumerable<Student> Students = controller.FetchAllStudents(searchKey);
                return View(Students);
            }
            catch (Exception ex)
            {
                // redirect to the error page
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("error", "Students");
            }
        }


        //GET : Students/Details/{id}
        public ActionResult Details(int id)
        {
            try
            {
                // return the student's details
                Student Student = controller.FetchStudentDetails(id);
                return View(Student);
            }
            catch (Exception ex)
            {
                // redirect to the error page
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("error", "Students");
            }
        }

        //GET : Students/Courses/{id}
        public ActionResult Courses(int id)
        {
            try
            {
               // return the list of classes of the sudent
                IEnumerable<Course> Courses = controller.fetchAllCoursesOfStudent(id);
                return View(Courses);
            }
            catch (Exception ex)
            {
                // redirect to the error page
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("error", "Students");
            }
        }
    }
}
