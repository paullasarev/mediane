using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediane.DomainModel
{
    public class QueryObject
    {
        public string ArticleByTitle
        {
            get { return "SELECT * FROM ARTICLES WHERE Title=@0"; }
        }

        public string ClearAll
        {
            get
            {
                return @"
                    DELETE FROM articles;
                ";
            }
        }
    }

}