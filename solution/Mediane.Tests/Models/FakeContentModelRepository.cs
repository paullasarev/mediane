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

    class FakeArticleRepository : IArticleRepository
    {
        protected Dictionary<string, Article> Models = new Dictionary<string, Article>();

        protected Article Create(string id)
        {
            var model = new ArticleImpl(id);
            model.Content = "New page template";
            return model;
        }

        public Article Load(string id)
        {
            string key = id.Trim();
            if (Models.ContainsKey(key))
            {
                return Models[key];
            }
            else
            {
                return Create(id);
            }
        }

        public void Save(Article model)
        {
            Models[model.Title] = model;

        }
    }
}
