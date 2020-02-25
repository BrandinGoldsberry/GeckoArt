using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectGecko.Models;

namespace ProjectGecko.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Posts = Post.GetAllPosts();
            if(SessionVars.ActiveAcount != null)
            {
                return View(SessionVars.ActiveAcount);
            }
            return View();
        }
    }
}