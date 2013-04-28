using Mediane.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mediane.Tests.Models
{
    class ContentModelImpl : ContentModel
    {
        public ContentModelImpl(string id)
            : base(id)
        {
        }
    }

    class FakeContentModelRepository : ContentModelRepository
    {
    }
}
