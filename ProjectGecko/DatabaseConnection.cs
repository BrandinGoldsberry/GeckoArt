using MongoDB.Bson;
using MongoDB.Driver;
using ProjectGecko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectGecko
{
    public class DatabaseConnection
    {
        private static IMongoDatabase accountDatabase;

        public static void DatabaseConnect()
        {
            accountDatabase = new MongoClient("mongodb+srv://admin:password1234@test-un7p6.azure.mongodb.net/test?retryWrites=true&w=majority&connect=replicaSet").GetDatabase("AccountDB");
        }

        public static Account GetAccount(long Id)
        {
            //return user by id
            return accountDatabase.GetCollection<Account>("AccountInfo").FindAsync(a => a.AccountID == Id).Result.FirstOrDefault();
        }

        public static Account GetAccount(string UserName)
        {
            //return user by id
            return accountDatabase.GetCollection<Account>("AccountInfo").FindAsync(a => a.UserName == UserName).Result.FirstOrDefault();
        }

        public static List<Account> GetAccounts(FilterDefinition<Account> FindAsyncFunc)
        {
            return accountDatabase.GetCollection<Account>("AccountInfo").FindAsync(FindAsyncFunc).Result.ToList();
        }

        public static void UpdateAccount(FilterDefinition<Account> fil, UpdateDefinition<Account> upd)
        {
            accountDatabase.GetCollection<Account>("AccountInfo").UpdateOneAsync(fil, upd);
        }

        public static Commission GetCommission(long Id)
        {
            return accountDatabase.GetCollection<Commission>("Commissions").FindAsync(c => c.CommissionID == Id).Result.FirstOrDefault();
        }

        public static List<Commission> GetCommissions(long commissiOneAsynceId)
        {
            return accountDatabase.GetCollection<Commission>("Commissions").FindAsync(c => c.CommissionID == commissiOneAsynceId).Result.ToList();
        }

        public static List<Commission> GetCommissionsFromUser(long userID)
        {
            return accountDatabase.GetCollection<Commission>("Commissions").FindAsync(x => true).Result.ToList().Where(x => x.CommissionID == userID).ToList(); ;
        }

        public static List<Commission> GetCommissionsForUser(long userID)
        {
            return accountDatabase.GetCollection<Commission>("Commissions").FindAsync(x => true).Result.ToList().Where(x => x.CommissioneeID == userID).ToList(); ;
        }

        public static Post GetPost(long Id)
        {
            return accountDatabase.GetCollection<Post>("Posts").FindAsync(a => a.PostID == Id).Result.FirstOrDefault();
        }

        public static List<Post> FindAsyncPosts(string Title)
        {
            return accountDatabase.GetCollection<Post>("Posts").FindAsync(p => p.Title.Contains(Title)).Result.ToList();
        }
        
        public static List<Account> FindAsyncAccounts(string Name)
        {
            return accountDatabase.GetCollection<Account>("AccountInfo").FindAsync(m => m.UserName.Contains(Name) || m.DisplayName.Contains(Name)).Result.ToList();
        }

        public static List<Post> GetAllPosts(bool reversed)
        {
            if (reversed)
            {
                var ret = accountDatabase.GetCollection<Post>("Posts").FindAsync(x => true).Result.ToList();
                ret.Reverse();
                return ret;
            }
            else
                return accountDatabase.GetCollection<Post>("Posts").FindAsync(x => true).Result.ToList();
        }

        public static List<Post> GetPosts(FilterDefinition<Post> FindAsyncFunc)
        {
            return accountDatabase.GetCollection<Post>("Posts").FindAsync(FindAsyncFunc).Result.ToList();
        }

        public static List<Post> GetUserPosts(long userID)
        {
            return accountDatabase.GetCollection<Post>("Posts").FindAsync(x => true).Result.ToList().Where(x => x.PosterID == userID).ToList();
        }

        public static BsonDocument RunCommand(JsonCommand<BsonDocument> command)
        {
            return accountDatabase.RunCommand(command);
        }

        public static void InsertAccount(Account a)
        {
            accountDatabase.GetCollection<Account>("AccountInfo").InsertOneAsync(a);
        }

        public static void InsertCommision(Commission c)
        {
            accountDatabase.GetCollection<Commission>("Commissions").InsertOneAsync(c);
        }
        public static void InsertPost(Post p)
        {
            accountDatabase.GetCollection<Post>("Posts").InsertOneAsync(p);
        }

        public static void UpdatePost(Post p)
        {
            accountDatabase.GetCollection<Post>("Posts").ReplaceOneAsync(X => X.PostID == p.PostID, p);
        }
        
        public static void UpdateAccount(Account a)
        {
            accountDatabase.GetCollection<Account>("AccountInfo").ReplaceOneAsync(X => X.AccountID == a.AccountID, a);
        }
        
        public static void UpdateCommission(Commission c)
        {
            accountDatabase.GetCollection<Commission>("Commissions").ReplaceOneAsync(X => X.CommissionID == c.CommissionID, c);
        }

        public static void RemoveCommission(long id)
        {
            accountDatabase.GetCollection<Commission>("Commissions").DeleteOneAsync(c => c.CommissionID == id);
        }
    }
}
