﻿using Assignment3_N01609602.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment3_N01609602.Controllers
{
    public class TeachersController : Controller
    {
        // GET: Teachers/list
        public ActionResult List()
        {
            TeachersDataController controller = new TeachersDataController();
            IEnumerable<Teacher> Teachers = controller.FetchAllTeachers();
            return View(Teachers);
        }

        //GET : Teachers/Details/{id}
        public ActionResult Details(int id)
        {
            TeachersDataController controller = new TeachersDataController();
            Teacher Teacher = controller.FetchTeacherDetails(id);
            return View(Teacher);
        }

    }
}