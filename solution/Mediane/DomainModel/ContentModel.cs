using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediane.DomainModel
{
    public class ContentModel
    {
        public string Id { get; private set; }

        protected ContentModel(string newId)
        {
            this.Id = newId.Trim();
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
    }
}