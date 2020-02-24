using ProjectGecko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectGecko
{
    public class SessionVars
    {
        public static Account ActiveAcount { get; set; }
        //public static List<Account> AccountTesting = new List<Account>();
        public static List<Post> posts = new List<Post>();
    }
}
