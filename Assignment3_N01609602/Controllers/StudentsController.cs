using Assignment3_N01609602.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment3_N01609602.Controllers
{
    public class StudentsController : Controller
    {
        // GET: Students/List/{searchKey}
        public ActionResult List(string searchKey = "")
        {
            StudentsDataController controller = new StudentsDataController();
            IEnumerable<Student> Students = controller.FetchAllStudents(searchKey);
            return View(Students);
        }


        //GET : Students/Details/{id}
        public ActionResult Details(int id)
        {
            StudentsDataController controller = new StudentsDataController();
            Student Student = controller.FetchStudentDetails(id);
            return View(Student);
        }

        //GET : Students/Classes/{id}
        public ActionResult Classes(int id)
        {
            StudentsDataController controller = new StudentsDataController();
            IEnumerable<Classes> Classes = controller.fetchAllClassesOfStudent(id);
            return View(Classes);
        }
    }
}
