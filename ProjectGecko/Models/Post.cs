using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectGecko.Models
{
    public class Post
    {
        private static int _LastID;

        public int PostID { get; set; }

        public string Title { get; set; }

        public int PosterID { get; set; }

        public string[] ImagePaths { get; set; }

        public List<Comment> Comments { get; set; }

        public Post()
        {
            PostID = _LastID++;
            Comments = new List<Comment>();
        }
    }
}
