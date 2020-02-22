using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectGecko.Models;

namespace ProjectGecko.Controllers
{
    public class PostController : Controller
    {
        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePost(string Title, List<IFormFile> files)
        {
            return View();
        }
    }
}