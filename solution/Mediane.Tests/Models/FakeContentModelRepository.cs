using Mediane.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mediane.Tests.Models
{
    class FakeArticleRepository : IArticleRepository
    {
        protected Dictionary<string, Article> Models = new Dictionary<string, Article>();

        protected Article Create(string id)
        {
            var model = new Article(id);
            model.Content = "New page template";
            model.IsNew = true;
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
            model.IsNew = false;
            Models[model.Title] = model;
        }
    }
}
