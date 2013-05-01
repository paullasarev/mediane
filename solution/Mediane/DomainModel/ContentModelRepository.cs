using Mediane.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediane.DomainModel
{
    class ArticleImpl : Article
    {
        public ArticleImpl(string id)
            : base(id)
        {
        }
    }


    public class ArticleRepository : IArticleRepository
    {
        protected Dictionary<string, Article> Models = new Dictionary<string,Article>();

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
            Models[model.Id] = model;
        }
    }
}