using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectGecko.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.RegularExpressions;
using MongoDB.Driver;

namespace ProjectGecko.Controllers
{
    public class UserController : Controller
    {
        private IMongoDatabase mongoDatabase;
        public IMongoDatabase GetMongoDatabase()
        {
            var mongoClient = new MongoClient("mongodb+srv://admin:password1234@test-un7p6.azure.mongodb.net/test?retryWrites=true&w=majority");
            return mongoClient.GetDatabase("AccountDB");
        }
        public IActionResult ShowFeed()
        {
            return View();
        }
        public IActionResult ShowAccount()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAccount(string DisplayName,
            string UserName,
            string PhoneNumber,
            string Email,
            string Password,
            IFormFile ProfPicPath)
        {
            //System.Diagnostics.Debug.Write("Image Path: " + ProfPicPath.FileName);

            bool userIsTaken = SessionVars.AccountTesting.Where(a => a.UserName == UserName).Count() != 0;

            if(userIsTaken)
            {
                ModelState.AddModelError("UserName", "UserName is taken");
            }

            //ensures that file is an image
            if (ProfPicPath.ContentType.ToString() != "image/png" &&
                ProfPicPath.ContentType.ToString() != "image/jpeg" &&
                ProfPicPath.ContentType.ToString() != "image/jpg"
                )
            {
                ModelState.AddModelError("ProfPicPath", "Image MUST be a png or jpeg (jpg)");
            }

            if(Regex.Match(UserName, @"\s").Success)
            {
                ModelState.AddModelError("UserName", "UserName may not contain spaces because it ruins file structure");
            }

            //Defaults Displayname to username if it was left blank
            DisplayName = string.IsNullOrWhiteSpace(DisplayName) ? UserName : DisplayName;
            
            if(ModelState.IsValid)
            {
                if(ProfPicPath != null)
                {
                    //Grabs extension from image
                    var match = Regex.Match(ProfPicPath.FileName, @"^.+(?<extension>\.[A-Za-z]+)$");
                    string ImageExtension = match.Groups["extension"].Value;
                    //constructs path for account image
                    string pathForImage = $"~/Images/Users/{UserName}/Profile/ProfilePicture" + ImageExtension;
                    //for local copy of image
                    string pathForCopy = $"wwwroot/Images/Users/{UserName}/Profile/ProfilePicture" + ImageExtension;

                    Directory.CreateDirectory($"wwwroot/Images/Users/{UserName}/Profile/");

                    //saves Image
                    using (FileStream stream = System.IO.File.OpenWrite(pathForCopy))
                    {
                        
                        ProfPicPath.CopyTo(stream);
                    }

                    Account account = new Account()
                    {
                        DisplayName = DisplayName,
                        UserName = UserName,
                        PhoneNumber = PhoneNumber,
                        Password = Password,
                        Email = Email,
                        ProfPicPath = pathForImage
                    };

                    mongoDatabase = GetMongoDatabase();
                    mongoDatabase.GetCollection<Account>("AccountInfo").InsertOne(account);
                    SessionVars.AccountTesting.Add(account);
                    SessionVars.ActiveAcount = account;
                }
                return Redirect("/");

            }
            return View();
        }
    }
}