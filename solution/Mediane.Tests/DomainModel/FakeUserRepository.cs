using Mediane.DomainModel;
using System;
using System.Collections.Generic;
using System.Web.Security;
using WebMatrix.WebData;

namespace Mediane.Tests.DomainModel
{
    public class FakeUserRepository : IUserRepository
    {
        protected Dictionary<string, string> Users = new Dictionary<string, string>();

        public bool Validate(string username, string password)
        {
            string pass;
            if (!Users.TryGetValue(username, out pass))
            {
                return false;
            }

            return pass == password;
        }

        public void Create(string username, string password)
        {
            if (Users.ContainsKey(username))
            {
                throw new MembershipCreateUserException();
            }

            Users[username] = password;
        }
    }
}
