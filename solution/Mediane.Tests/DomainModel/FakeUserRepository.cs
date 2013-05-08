using Mediane.DomainModel;
using System;
using System.Collections.Generic;
using System.Web.Security;
using WebMatrix.WebData;

namespace Mediane.Tests.DomainModel
{
    public class FakeUserRepository : IUserRepository
    {
        protected struct User
        {
            public string Password;
            public int UserId;
        }

        protected Dictionary<string, User> Users = new Dictionary<string, User>();
        
        static int currentUserId = 1;
        static int NextUserId() 
        {
            int ret = currentUserId;
            ++currentUserId;
            return ret;
        }

        public bool Validate(string username, string password)
        {
            User user;
            if (!Users.TryGetValue(username, out user))
            {
                return false;
            }

            return user.Password == password;
        }

        public void Create(string username, string password)
        {
            if (Users.ContainsKey(username))
            {
                throw new MembershipCreateUserException();
            }

            Users[username] = new User { Password = password, UserId = NextUserId() };
        }

        public int GetUserId(string username)
        {
            if (!Users.ContainsKey(username))
            {
                throw new ArgumentException();
            }

            return Users[username].UserId;
        }

        public string GetUserById(int id)
        {
            foreach (var e in Users)
            {
                if (e.Value.UserId == id)
                {
                    return e.Key;
                }
            }

            return null;
        }

    }
}
