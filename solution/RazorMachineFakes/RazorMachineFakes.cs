using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xipton.Razor;

namespace RazorMachineFakes
{
    // see http://www.codeproject.com/Articles/423141/Razor-2-0-template-engine-supporting-layouts

    public static class Styles
    {
        public static LiteralString Render(string what)
        {
            var result = "<link href=\"" + what.Remove(0, 1) + "\" rel=\"stylesheet\"/>";
            return new LiteralString(result);
        }
    }

    public static class Scripts
    {
        public static LiteralString Render(string what)
        {
            var result = "<script src=\"" + what.Remove(0, 1) + "\"></script>";
            return new LiteralString(result);
        }
    }

    public class FakeBeginForm : IDisposable
    {
        public FakeBeginForm(
            string actionName,
            string controllerName,
            FormMethod method)
        {
        }

        public void Dispose()
        {
        }
    }

    public static class Html
    {
        public static LiteralString ActionLink(
            string linkText,
            string actionName,
            string controllerName,
            object routeValues = null,
            object htmlAttributes = null)
        {
            string attrs = "";
            //if (htmlAttributes != null)
            //{
            //}

            var result = "<a href=\"/" + controllerName + "/" + actionName + "\"" + attrs + "> " + linkText + "</a>";
            return new LiteralString(result);
        }


        public static LiteralString RouteLink(
            string linkText,
            string routeName,
            object routeValues = null,
            object htmlAttributes = null)
        {
            string attrs = "";
            //if (htmlAttributes != null)
            //{
            //}

            var result = "<a href=\"/" + routeName + "/" + routeName + "\"" + attrs + "> " + linkText + "</a>";
            return new LiteralString(result);
        }

        public static LiteralString Partial(
            string partialViewName,
            Object model = null,
            ViewDataDictionary viewData = null)
        {
            var result = "<partial>" + partialViewName + "</partial>";
            return new LiteralString(result);
        }

        public static LiteralString Raw(string what)
        {
            return new LiteralString(what);
        }

        public static FakeBeginForm BeginForm(
            string actionName,
            string controllerName,
            FormMethod method,
            object htmlAttributes = null)
        {
            return new FakeBeginForm(actionName, controllerName, method);
        }

        public static FakeBeginForm BeginForm(
            string actionName,
            string controllerName,
            object routeValues,
            FormMethod method,
            object htmlAttributes)
        {
            return new FakeBeginForm(actionName, controllerName, method);
        }

        public static LiteralString TextArea(
            string name, string value, int nrows = 20, int ncolumns = 60, object htmlAttributes = null)
        {
            var result = "<input type=\"text\" name=\"" + name + "\">" + value + "</input> ";
            return new LiteralString(result);
        }

        public static LiteralString ViewMenu(object model, string viewName)
        {
            return new LiteralString("<li><a>View</a></li>");
        }


        public static LiteralString DropDownList(string name,
            IEnumerable<SelectListItem> selectList,
            string optionLabel = "")
        {
            return new LiteralString("<li><a>DropDownList: " + name + "</a></li>");
        }
    }
}
