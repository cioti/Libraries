using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Libraries.WebApi.ResponseWrapper
{
    public class IgnoreWrap : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) 
        {
            context.HttpContext.Response.Headers.Add(Constants.IgnoreWrap, "true");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
