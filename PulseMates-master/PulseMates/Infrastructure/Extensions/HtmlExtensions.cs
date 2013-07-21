namespace PulseMates.Infrastructure.Extensions
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

    public static class HtmlExtensions
    {
        public static MvcHtmlString Title(this HtmlHelper helper, string title, string message = "")
        {
            if (!HasEnding(title))
                title += ".";

            if (!string.IsNullOrEmpty(message) && !HasEnding(message))
                message += ".";

            return new MvcHtmlString(string.Format(@"<div class=""title"">
                <h1>{0}</h1>
                <h2>{1}</h2>
            </div>", title, message));
        }
        public static MvcHtmlString TitleFor(this HtmlHelper helper)
        {
            return Title(helper, helper.ViewBag.Title, helper.ViewBag.Message);
        }

        private static bool HasEnding(string title)
        {
            return title.EndsWith(".") || title.EndsWith("?") || title.EndsWith("!");
        }

        public static MvcHtmlString Usage(this HtmlHelper helper, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                return new MvcHtmlString(string.Format(@"
                    <span class=""usage"">?</span>
                    <div class=""usage-message"" style=""display: none"">{0}</div>", message));
            }

            return new MvcHtmlString("");
        }

        public static MvcHtmlString UsageFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> exp)
        {
            string propertyName = (exp.Body as MemberExpression) != null ? 
                (exp.Body as MemberExpression).Member.Name : "";

            try
            {
                return Usage(helper, (string)typeof(TModel).GetProperty(propertyName).GetValue(helper.ViewData.Model));
            }
            catch { return Usage(helper, ""); }
        }

    }
}