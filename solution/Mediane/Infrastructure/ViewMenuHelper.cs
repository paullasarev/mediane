using Mediane.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mediane.Infrastructure
{
    public static class ViewMenuHelper
    {
        public static MvcHtmlString ViewMenu(this HtmlHelper helper, Article model, string viewName)
        {
            string result = @"
            <li><a href=""#"" class=""here"">Document</a></li>
            <li><a href=""#"">Comment</a></li>
            <li><a href=""#"">Property</a></li>
            <li><a href=""#"">Attach</a></li>
            ";

            return MvcHtmlString.Create(result);
        }
    }
}