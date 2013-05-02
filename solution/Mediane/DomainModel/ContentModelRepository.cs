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

    [PetaPoco.TableName("articles")]
    [PetaPoco.PrimaryKey("ArticleId")]
    public class ArticleDb
    {
        public long ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class ArticleRepository : IArticleRepository
    {
        private PetaPoco.Database Db;

        public ArticleRepository(PetaPoco.Database Db)
        {
            this.Db = Db;
        }

        public ArticleRepository(string connectionString, string providerName)
        {
            this.Db = new PetaPoco.Database(connectionString, providerName);
        }

        public ArticleRepository(string connectionStringName)
        {
            this.Db = new PetaPoco.Database(connectionStringName);
        }

        protected Article Create(string id)
        {
            var model = new ArticleImpl(id);
            model.Content = "New page template";
            return model;
        }

        public Article Load(string id)
        {
            //string key = id.Trim();
            //if (Models.ContainsKey(key))
            //{
            //    return Models[key];
            //}
            //else
            //{
            //    return Create(id);
            //}
            return new ArticleImpl(id);
        }

        public void Save(Article model)
        {
            var dbModel = new ArticleDb(); ;
            dbModel.Title = model.Title;
            dbModel.Content = model.Content;

            Db.Insert(dbModel);
        }
    }
}