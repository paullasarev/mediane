using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediane.DomainModel
{
    public class Article
    {
        public string Title { get; private set; }

        protected Article(string title)
        {
            this.Title = title.Trim();
        }

        public string Rendered
        {
            get
            {
                return Render(Content);
            }
        }

        private string ContentValue;

        public string Content
        {
            get
            {
                return ContentValue;
            }

            set
            {
                if (value != null)
                {
                    ContentValue = value;
                }
            }
        }

        public string Render(string content)
        {
            return "<p>" + content + "</p>";
        }

        public bool IsNew { get; set; }
    }
}