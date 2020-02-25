using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectGecko.Models
{
    public class Commission
    {
        //MongoDB ObjectID (This is required by Mongo)
        public ObjectId Id;

        //User commissioning the art
        public long CommissionerID { get; set; }
        
        //User being commissioned for art
        public long CommissioneeID { get; set; }

        //An array of all the image paths given by the user
        public string[] ImagePaths { get; set; }

        //This is the summary of what the commissioner wants from the artist
        public string Description { get; set; }
    }
}
