using MyAZHRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAZHRM.Controllers
{
    public class ErrorController : Controller
    {
        // Return the 404 not found page   
        public ActionResult Error404()
        {
            return View();
        }

        // Return the 500 not found page   
        public ActionResult Error500()
        {
            return View();
        }

    }
}
