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
        void Create(string username, string password);
        int GetUserId(string username);

        string GetUserById(int id);
    }
}
