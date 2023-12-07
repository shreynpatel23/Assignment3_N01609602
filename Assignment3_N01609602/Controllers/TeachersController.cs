using Assignment3_N01609602.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Assignment3_N01609602.Controllers
{
    public class TeachersController : Controller
    {
        // declare the controller here once and for all
        TeachersDataController controller = new TeachersDataController();

        // GET: Teachers/error?message={message}
        public ActionResult Error([FromUri] string message)
        {
            ViewBag.errorMessage = message;
            return View();
        }

        // GET: Teachers/list/{searchKey}
        public ActionResult List(string searchKey = "")
        {
            try
            {
                // get a list of Teachers.
                IEnumerable<Teacher> Teachers = controller.FetchAllTeachers(searchKey);
                return View(Teachers);
            }
            catch (Exception ex)
            {
                // redirect to the error page
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("error", "Teachers");
            }
        }

        //GET : Teachers/Details/{id}
        public ActionResult Details(int id)
        {
            try
            {
                // fetch teacher details
                Teacher Teacher = controller.FetchTeacherDetails(id);
                return View(Teacher);
            }
            catch (Exception ex)
            {
                // redirect to the error page
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("error", "Teachers");
            }
        }

        //GET : Teachers/Classes/{id}
        public ActionResult Classes(int id)
        {
            try
            {
                // fetch all classes
                IEnumerable<Classes> Classes = controller.fetchAllClaasesOfTeacher(id);
                return View(Classes);
            }
            catch (Exception ex)
            {
                // redirect to the error page
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("error", "Teachers");
            }
        }

        //GET : Teachers/Confirm/{id}
        public ActionResult Confirm(int id)
        {
            try
            {
                // fetch teacher details
                Teacher Teacher = controller.FetchTeacherDetails(id);
                return View(Teacher);
            }
            catch (Exception ex)
            {
                // redirect to the error page
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("error", "Teachers");
            }
        }

        //GET : Teachers/Add
        public ActionResult Add()
        {
            try
            {
                // return the view
                return View();
            }
            catch (Exception ex)
            {
                // redirect to the error page
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("error", "Teachers");
            }
        }

        // GET Teachers/Edit/{id}
        public ActionResult Edit(int id)
        {
            try
            {
                // get the existing teacher details
                Teacher newTeacher = controller.FetchTeacherDetails(id);

                // here the salary is stored in decimal format in the databse 
                // so we need to convert it into int so that .00 is added in the input box
                // while updating the salary of the teacher
                ViewBag.salary = Convert.ToInt32(newTeacher.salary);
                // return the view
                return View(newTeacher);
            }
            catch (Exception ex)
            {
                // redirect to the error page
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("error", "Teachers");
            }
        }

    }
}
