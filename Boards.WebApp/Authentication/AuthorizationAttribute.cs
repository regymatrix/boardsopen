using Boards.WebApp.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boards.WebApp.Authentication
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class)]
    public class AuthorizationAttribute : ActionFilterAttribute
    {
     
        public AuthorizationAttribute() 
        {
        
        }
       
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!new AuthHandler(context.HttpContext).IsUsuarioLogado())
            {
                context.HttpContext.Response.StatusCode = 501;
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (new AuthHandler(context.HttpContext).IsUsuarioLogado())
            {
                context.Canceled = false;
            }
            else
            {
                context.HttpContext.Response.StatusCode = 501;
                context.Canceled = true;
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
