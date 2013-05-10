using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Mediane.DomainModel
{
    [PetaPoco.TableName("Users")]
    [PetaPoco.PrimaryKey("UserId", autoIncrement = true)]
    public class UserDb
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        //public string Password { get; set; }
        //[PetaPoco.ResultColumn]
    }

    [PetaPoco.TableName("webpages_Membership")]
    [PetaPoco.PrimaryKey("UserId", autoIncrement = false)]
    public class MembershipDb
    {
        public int UserId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ConfirmationToken { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<System.DateTime> LastPasswordFailureDate { get; set; }
        public int PasswordFailuresSinceLastSuccess { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> PasswordChangedDate { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordVerificationToken { get; set; }
        public Nullable<System.DateTime> PasswordVerificationTokenExpirationDate { get; set; }
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
            var userDb = Db.SingleOrDefault<UserDb>(Query.UserByUsername, username);
            if (userDb == null)
            {
                return false;
            }

            var userId = userDb.UserId;

            var membershipDb = Db.SingleOrDefault<MembershipDb>(userId);
            if (membershipDb == null)
            {
                return false;
            }

            return membershipDb.Password == password;
        }

        public int CreateLocal(string username, string password)
        {
            var userDb = new UserDb() {
                UserName = username,
            };

            Db.Insert(userDb);
            var userId = userDb.UserId;

            var membershipDb = new MembershipDb() {
                UserId = userId,
                IsConfirmed = true,
                CreateDate = DateTime.Now,
                Password = password,
                PasswordSalt = "",
            };

            Db.Insert(membershipDb);

            return userId;
        }

        public int GetUserId(string username)
        {
            UserDb user = Db.SingleOrDefault<UserDb>(Query.UserByUsername, username);
            if (user == null)
            {
                return -1;
            }

            return user.UserId;
        }

        public string GetUserById(int id)
        {
            UserDb user = Db.SingleOrDefault<UserDb>(Query.UserByUserId, id);
            if (user == null)
            {
                return null;
            }

            return user.UserName;
        }



        public bool UserExist(string username)
        {
            return GetUserId(username) != -1;
        }

        public int CreateUser(string username)
        {
            var userDb = new UserDb()
            {
                UserName = username,
            };

            Db.Insert(userDb);
            return userDb.UserId;
        }
    }
}