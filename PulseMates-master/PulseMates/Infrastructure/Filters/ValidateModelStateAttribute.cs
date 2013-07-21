namespace PulseMates.Infrastructure.Filters
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;

            if (!modelState.IsValid)
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
            //else if (IsFormBodyNull(actionContext))
            //    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Request has no body content");
        }

        private bool IsFormBodyNull(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetParameters()
                .Where(x => x.ParameterBinderAttribute != null && actionContext.ActionArguments[x.ParameterName] == null)
                .FirstOrDefault() != null;
        }
    }
}