using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Raintels.Entity.DataModel;
using Raintels.Entity.ViewModel;
using Raintels.Service.Service;
using Raintels.Service.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raintels.CHBuddy.Web.API
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            var idToken = context.HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "Authorization").Value;
            var token = idToken.ToString().Replace("Bearer", "").Trim();
            var defaultAuth = FirebaseAuth.DefaultInstance;
            var decodedToken = await defaultAuth.VerifyIdTokenAsync(token);
            if (decodedToken == null || decodedToken.Claims == null ||
                string.IsNullOrEmpty(decodedToken.Claims["email"].ToString()))
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }

    }
}
