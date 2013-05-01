using Mediane.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediane.DomainModel
{
    public interface IArticleRepository
    {
        Article Load(string id);
        void Save(Article model);
    }
}
