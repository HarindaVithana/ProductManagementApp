using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAZHRM.Controllers
{
    //[HandleError]
    public class HomeController : Controller
    {
        //[HandleError(View = "CustomErrorView")]
        public ActionResult ThrowException()
        {
            throw new ApplicationException();
        }

        //[HandleError]
        public ActionResult Index()
        {
            // ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View();
        }

        //[HandleError]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        //[HandleError]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
