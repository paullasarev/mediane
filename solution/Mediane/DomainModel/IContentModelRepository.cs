using Mediane.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediane.DomainModel
{
    public interface IContentModelRepository
    {
        ContentModel Create(string id);
        ContentModel Load(string id);
        void Save(ContentModel model);
    }
}
