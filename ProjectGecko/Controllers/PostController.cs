using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProjectGecko.Models;

namespace ProjectGecko.Controllers
{
    public class PostController : Controller
    {
        [HttpGet]
        public IActionResult ShowPost(long postid)
        {
            return View(Post.GetPost(postid));
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePost(string Title, IFormFileCollection ImagePaths, int userid)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            if(string.IsNullOrWhiteSpace(Title))
            {
                ModelState.AddModelError("Title", "Please enter a title");
            }
            else
            {
                if (ImagePaths.Any())
                {
                    Account AccountPoster = Account.GetAccount(userid);
                    string[] pathList = new string[ImagePaths.Count];
                    Post newPost = new Post()
                    {
                        Title = Title,
                        PosterID = userid
                    };

                    Directory.CreateDirectory($"wwwroot/Images/Users/{AccountPoster.UserName}/Posts/{newPost.PostID}");
                    int i = 0;
                    foreach (var item in ImagePaths)
                    {
                        
                        var match = Regex.Match(item.FileName, @"^.+(?<extension>\.[A-Za-z]+)$");
                        string ImageExtension = match.Groups["extension"].Value;
                        string imageName = Regex.Replace(item.FileName, @"\s", "+");
                        //constructs path for post storage image
                        string pathForImage = $"~/Images/Users/{AccountPoster.UserName}/Posts/{newPost.PostID}/Image" + i + ImageExtension;
                        //for local copy of image
                        string pathForCopy = $"wwwroot/Images/Users/{AccountPoster.UserName}/Posts/{newPost.PostID}/Image" + i + ImageExtension;
                        pathList[i] = pathForImage;
                        using (FileStream stream = System.IO.File.OpenWrite(pathForCopy))
                        {

                            item.CopyTo(stream);
                        }
                        i++;
                    }

                    newPost.ImagePaths = pathList;

                    //System.Diagnostics.Debug.Write(userid);

                    var mongoClient = new MongoClient("mongodb+srv://admin:password1234@test-un7p6.azure.mongodb.net/test?retryWrites=true&w=majority&connect=replicaSet").GetDatabase("AccountDB");
                    mongoClient.GetCollection<Post>("Posts").InsertOne(newPost);

                    return Redirect($"/{newPost.PostID}/ShowPost");
                }
                else
                {
                    Post newPost = new Post()
                    {
                        Title = Title,
                        PosterID = userid
                    };

                    var mongoClient = new MongoClient("mongodb+srv://admin:password1234@test-un7p6.azure.mongodb.net/test?retryWrites=true&w=majority&connect=replicaSet").GetDatabase("AccountDB");
                    mongoClient.GetCollection<Post>("Posts").InsertOne(newPost);

                    return Redirect($"/{newPost.PostID}/ShowPost");
                }
            }
            return View();
        }
    }
}