using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectGecko.Models
{
    public class Account
    {
        private static int _lastID = 0;
        public int AccountID { get; set; }
        //Account display name
        public string DisplayName { get; set; }

        //Static username
        [Required(ErrorMessage = "Please enter a Permamnent UserName")]
        [StringLength(16, ErrorMessage = "Please enter a nmae between 5 and 16 Characters", MinimumLength = 5)]
        public string UserName { get; set; }

        //User password (one special, one number, one capitol, and at least 8 characters)
        [Required(ErrorMessage = "Please enter a password")]
        [RegularExpression(@"^(?=.*[!@#$%^&*()\[\]{};:' <>,.\/?])(?=.*[A-Z])(?=.*[0-9]).{8,}$", ErrorMessage = "one special, one number, one capitol, and at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //Query for getting img from database
        [DataType(DataType.Upload)]
        public string ProfPicPath { get; set; }

        //Phone number for contact
        [RegularExpression(@"^\d?(\s|-)?(\(\d{3}\)|\d{3})(\s|-)?\d{3}(\s|-)?\d{4}?")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        //Email for contact
        [RegularExpression(@"((\w|\d)+)@((\w|\d){2,})\.((\w|\d){2,})")]
        [DataType(DataType.EmailAddress)]
        public string  Email { get; set; }

        //user's customized bio
        [DataType(DataType.MultilineText)]
        public string Biography { get; set; }

        //user's paypal (examplary for this project)
        [DataType(DataType.EmailAddress)]
        public string PayPal { get; set; }

        public Account()
        {
            AccountID = _lastID++;
        }

        public static Account GetAccount(int Id)
        {
            //connect to mongodb
            var mongoClient = new MongoClient("mongodb+srv://admin:password1234@test-un7p6.azure.mongodb.net/test?retryWrites=true&w=majority").GetDatabase("AccountDB");
            //return user by id
            return mongoClient.GetCollection<Account>("AccountInfo").Find(a => a.AccountID == Id).First();
        }
    }
}
