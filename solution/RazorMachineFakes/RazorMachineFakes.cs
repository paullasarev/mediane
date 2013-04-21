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

    public static class Html
    {
        // Summary:
        //     Returns an anchor element (a element) that contains the virtual path of the
        //     specified action.
        //
        // Parameters:
        //   htmlHelper:
        //     The HTML helper instance that this method extends.
        //
        //   linkText:
        //     The inner text of the anchor element.
        //
        //   actionName:
        //     The name of the action.
        //
        //   controllerName:
        //     The name of the controller.
        //
        //   routeValues:
        //     An object that contains the parameters for a route. The parameters are retrieved
        //     through reflection by examining the properties of the object. The object
        //     is typically created by using object initializer syntax.
        //
        //   htmlAttributes:
        //     An object that contains the HTML attributes to set for the element.
        //
        // Returns:
        //     An anchor element (a element).
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The linkText parameter is null or empty.
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


        public static LiteralString Partial(
            string partialViewName,
            Object model = null,
            ViewDataDictionary viewData = null)
        {
            var result = "<partial>" + partialViewName + "</partial>";
            return new LiteralString(result);
        }
    }
}
