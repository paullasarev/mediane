using Mediane.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediane.DomainModel
{
    class ArticleImpl : Article
    {
        public ArticleImpl(string title)
            : base(title)
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

    class QueryBuilder
    {
        public string ArticleByTitle
        {
            get { return "SELECT * FROM ARTICLES WHERE Title=@0"; }
        }
    }

    public class ArticleRepository : IArticleRepository
    {
        private PetaPoco.Database Db;
        private QueryBuilder Query = new QueryBuilder();

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

        public Article Create(string title)
        {
            var model = new ArticleImpl(title);
            model.IsNew = true;
            model.Content = "New page template";
            return model;
        }

        public Article Load(string title)
        {
            ArticleImpl a = new ArticleImpl(title);
            ArticleDb aDb = Db.SingleOrDefault<ArticleDb>(Query.ArticleByTitle, a.Title);
            if (aDb == null)
            {
                a.IsNew = true;
                a.Content = "";
            }
            else
            {
                a.Content = aDb.Content;
            }
            
            return a;
        }

        public void Save(Article model)
        {
            ArticleDb dbModel = Db.SingleOrDefault<ArticleDb>(Query.ArticleByTitle, model.Title);
            if (dbModel == null)
            {
                dbModel = new ArticleDb(); ;
                dbModel.Title = model.Title;
                dbModel.Content = model.Content;
                Db.Insert(dbModel);
            }
            else
            {
                dbModel.Content = model.Content;
                Db.Update(dbModel);
            }
        }
    }
}