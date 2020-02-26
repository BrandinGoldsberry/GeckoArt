using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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

        [HttpGet]
        public IActionResult LogInSignUp()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.Posts = null;
            ViewBag.Accounts = null;
            return View();
        }

        [HttpPost]
        public IActionResult Search(string SearchBar)
        {
            var mongoClient = new MongoClient("mongodb+srv://admin:password1234@test-un7p6.azure.mongodb.net/test?retryWrites=true&w=majority&connect=replicaSet").GetDatabase("AccountDB");
            ViewBag.Posts = mongoClient.GetCollection<Post>("Posts").Find(m => m.Title.Contains(SearchBar)).ToList();
            ViewBag.Accounts = mongoClient.GetCollection<Account>("AccountInfo").Find(m => m.UserName.Contains(SearchBar) || m.DisplayName.Contains(SearchBar)).ToList();
            return View();
        }
    }
}