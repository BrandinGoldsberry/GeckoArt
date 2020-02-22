using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProjectGecko.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if(SessionVars.ActiveAcount != null)
            {
                ViewBag["Posts"] = SessionVars.posts;
                return View(SessionVars.ActiveAcount);
            }
            return View();
        }
    }
}