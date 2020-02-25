using MongoDB.Bson;
using MongoDB.Driver;
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

        //The ID of the commission
        public long CommissionID { get; set; }

        //User commissioning the art
        public long CommissionerID { get; set; }
        
        //User being commissioned for art
        public long CommissioneeID { get; set; }

        //An array of all the image paths given by the user
        public string[] ImagePaths { get; set; }

        //This is the summary of what the commissioner wants from the artist
        public string Description { get; set; }

        public Commission()
        {
            var mongoClient = new MongoClient("mongodb+srv://admin:password1234@test-un7p6.azure.mongodb.net/test?retryWrites=true&w=majority").GetDatabase("AccountDB");
            long idCount = mongoClient.GetCollection<Account>("Commission").CountDocuments(a => true);
            CommissionID = idCount;
        }


    }
}
