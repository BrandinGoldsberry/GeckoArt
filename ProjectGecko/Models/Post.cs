using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectGecko.Models
{
    public class Post
    {
        public long PostID { get; set; }

        public string Title { get; set; }

        public long PosterID { get; set; }

        public string[] ImagePaths { get; set; }

        public List<Comment> Comments { get; set; }

        public Post()
        {
            var mongoClient = new MongoClient("mongodb+srv://admin:password1234@test-un7p6.azure.mongodb.net/test?retryWrites=true&w=majority").GetDatabase("AccountDB");
            //return user by id
            long idCount = mongoClient.GetCollection<Post>("Posts").CountDocuments(a => true);

            PostID = idCount;
            Comments = new List<Comment>();
        }

        public static Post GetPost(long Id)
        {
            //connect to mongodb
            var mongoClient = new MongoClient("mongodb+srv://admin:password1234@test-un7p6.azure.mongodb.net/test?retryWrites=true&w=majority").GetDatabase("AccountDB");
            //return user by id
            return mongoClient.GetCollection<Post>("Posts").Find(a => a.PostID == Id).First();
        }

        public Account GetPostAccount()
        {
            //connect to mongodb
            var mongoClient = new MongoClient("mongodb+srv://admin:password1234@test-un7p6.azure.mongodb.net/test?retryWrites=true&w=majority").GetDatabase("AccountDB");
            //return user by id
            return mongoClient.GetCollection<Account>("Account").Find(a => a.AccountID == PosterID).First();
        }
    }
}
