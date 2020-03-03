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
        public IActionResult ShowFeed()
        {
            return View();
        }
        public IActionResult ShowAccount(long userid)
        {
            Account user = Account.GetAccount(userid);
            ViewBag.Posts = DatabaseConnection.GetUserPosts(user.AccountID);
            return View(user);
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
            IFormFile profileImage)
        {
            if (string.IsNullOrWhiteSpace(UserName) || Regex.Match(UserName, @"\s").Success)
            {
                ModelState.AddModelError("UserName", "UserName is Invalid. Username cannot be empty or have spaces within it");
                return Redirect("LogInSignUp");
            }
            else if (string.IsNullOrWhiteSpace(Email))
            {
                ModelState.AddModelError("Email", "Email is Required");
                return Redirect("LogInSignUp");
            }
            else if (string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError("Password", "Password is Invaild");
                return Redirect("LogInSignUp");
            }
            else
            {
                bool userIsTaken = DatabaseConnection.GetAccount(UserName) != null;

                if (userIsTaken)
                {
                    ModelState.AddModelError("UserName", "UserName is taken");
                    return Redirect("LogInSignUp");
                }
                else if (Regex.Match(UserName, @"\s").Success)
                {
                    ModelState.AddModelError("UserName", "UserName may not contain spaces because it ruins file structure");
                    return Redirect("LogInSignUp");
                }
                else
                {
                    DisplayName = string.IsNullOrWhiteSpace(DisplayName) ? UserName : DisplayName;
                    if (ModelState.IsValid)
                    {
                        if (profileImage == null)
                        {
                            string pathForImage = "~/Images/Users/Default/defaultprofile.png";
                            Account account = new Account()
                            {
                                DisplayName = DisplayName,
                                UserName = UserName,
                                PhoneNumber = PhoneNumber,
                                Password = Password,
                                Email = Email,
                                ProfPicPath = pathForImage
                            };
                            DatabaseConnection.InsertAccount(account);
                            SessionVars.ActiveAcount = account;
                            return Redirect("/");
                        }
                        else if (profileImage.ContentType.ToString() != "image/png" && profileImage.ContentType.ToString() != "image/jpeg" && profileImage.ContentType.ToString() != "image/jpg")
                        {
                            string pathForImage = "~/Images/Users/Default/defaultprofile.png";
                            Account account = new Account()
                            {
                                DisplayName = DisplayName,
                                UserName = UserName,
                                PhoneNumber = PhoneNumber,
                                Password = Password,
                                Email = Email,
                                ProfPicPath = pathForImage
                            };
                            DatabaseConnection.InsertAccount(account);
                            SessionVars.ActiveAcount = account;
                            return Redirect("/");
                        }
                        else
                        {
                            //Grabs extension from image
                            var match = Regex.Match(profileImage.FileName, @"^.+(?<extension>\.[A-Za-z]+)$");
                            string ImageExtension = match.Groups["extension"].Value;
                            //constructs path for account image
                            string pathForImage = $"~/Images/Users/{UserName}/Profile/ProfilePicture" + ImageExtension;
                            //for local copy of image
                            string pathForCopy = $"wwwroot/Images/Users/{UserName}/Profile/ProfilePicture" + ImageExtension;

                            Directory.CreateDirectory($"wwwroot/Images/Users/{UserName}/Profile/");

                            //saves Image
                            using (FileStream stream = System.IO.File.OpenWrite(pathForCopy))
                            {
                                profileImage.CopyTo(stream);
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
                            DatabaseConnection.InsertAccount(account);
                            SessionVars.ActiveAcount = account;
                            return Redirect("/");
                        }
                    }
                    return Redirect("LogInSignUp");
                }
            }
        }

        [HttpGet]
        public IActionResult RequestCommission()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RequestCommission(long commissionerID, long commissioneeID, IFormFileCollection imagePaths, string description)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                ModelState.AddModelError("Description", "Please enter a description");
            }
            else
            {
                if (imagePaths.Any())
                {
                    Commission newCommission = new Commission();
                    Account AccountPoster = Account.GetAccount(commissionerID);
                    string[] pathList = new string[imagePaths.Count];

                    int i = 0;
                    foreach (var item in imagePaths)
                    {
                        if (item.ContentType.ToString() != "image/png" &&
                            item.ContentType.ToString() != "image/jpeg" &&
                            item.ContentType.ToString() != "image/jpg")
                        {
                            ModelState.AddModelError("imagePaths", "All images MUST be a png or jpeg (jpg)");
                            break;
                        }
                        else
                        {
                            var match = Regex.Match(item.FileName, @"^.+(?<extension>\.[A-Za-z]+)$");
                            string ImageExtension = match.Groups["extension"].Value;
                            string imageName = Regex.Replace(item.FileName, @"\s", "+");
                            string pathForImage = $"~/Images/Users/{AccountPoster.UserName}/Commissions/{commissionerID}/{commissioneeID}/{newCommission.CommissionID}/Image" + i + ImageExtension;
                            //for local copy of image
                            string pathForCopy = $"wwwroot/Images/Users/{AccountPoster.UserName}/Commissions/{commissionerID}/{commissioneeID}/{newCommission.CommissionID}/Image" + i + ImageExtension;
                            pathList[i] = pathForImage;
                            using (FileStream stream = System.IO.File.OpenWrite(pathForCopy))
                            {
                                item.CopyTo(stream);
                            }

                            i++;

                            if (imagePaths.Count() == i)
                            {
                                newCommission.CommissionerID = commissionerID;
                                newCommission.CommissioneeID = commissioneeID;
                                newCommission.ImagePaths = pathList;
                                newCommission.Description = description;
                            }
                        }
                    }

                    if (newCommission.CommissionerID == commissionerID && newCommission.CommissioneeID == commissioneeID && newCommission.ImagePaths == pathList && newCommission.Description == description)
                    {
                        DatabaseConnection.InsertCommision(newCommission);

                        //return Redirect($"/{newCommission.PostID}/ShowPost");
                        return Redirect("ShowFeed");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(string UserName, string Password)
        {
            Account LoginAttempt = Account.GetAccount(UserName);
            if (LoginAttempt == null)
            {
                ModelState.AddModelError("UserName", "Username Entered was incorrect");
                ModelState.AddModelError("Password", "Password Entered was incorrect");
                return Redirect("LogInSignUp");
            }
            else if (LoginAttempt.Password == Password)
            {
                SessionVars.ActiveAcount = LoginAttempt;
                return Redirect("/");
            }
            else
            {
                ModelState.AddModelError("Password", "Password Entered was incorrect");
                return Redirect("LogInSignUp");
            }
        }
    
        [HttpPost]
        public IActionResult Comment(string CommentText, string postId, string user)
        {
            Post p = Post.GetPost(long.Parse(postId));
            Comment c = new Comment() { CommentPoster = long.Parse(user), Text = CommentText };
            p.Comments.Add(c);
            DatabaseConnection.UpdatePost(p);
            return Redirect($"/{postId}/showpost");
        }
    }
}