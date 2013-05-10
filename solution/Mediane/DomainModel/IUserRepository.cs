using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediane.DomainModel
{
    public interface IUserRepository
    {
        bool Validate(string username, string password);
        int CreateLocal(string username, string password);
        int GetUserId(string username);

        string GetUserById(int id);

        bool UserExist(string username);

        int CreateUser(string username);
    }
}
