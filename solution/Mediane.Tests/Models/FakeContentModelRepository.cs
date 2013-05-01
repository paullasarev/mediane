using Mediane.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mediane.Tests.Models
{
    class ArticleImpl : Article
    {
        public ArticleImpl(string id)
            : base(id)
        {
        }
    }

    class FakeArticleRepository : ArticleRepository
    {
    }
}
