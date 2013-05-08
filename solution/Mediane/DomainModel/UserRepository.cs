using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediane.DomainModel
{
    [PetaPoco.TableName("Users")]
    [PetaPoco.PrimaryKey("Username", autoIncrement = false)]
    public class UserDb
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserRepository : IUserRepository
    {
        private PetaPoco.Database Db;
        private MedianeSql Query = new MedianeSql();

        public UserRepository(string connectionStringName)
        {
            this.Db = new PetaPoco.Database(connectionStringName);
        }

        public UserRepository(string connectionString, string providerName)
        {
            this.Db = new PetaPoco.Database(connectionString, providerName);
        }

        public bool Validate(string username, string password)
        {
            UserDb user = Db.SingleOrDefault<UserDb>(Query.UserByUsername, username);
            if (user == null)
            {
                return false;
            }

            return user.Password == password;
        }

        public void Create(string username, string password)
        {
            var m = new UserDb() { 
                Username = username, 
                Password = password 
            };

            Db.Insert(m);
        }
    }
}