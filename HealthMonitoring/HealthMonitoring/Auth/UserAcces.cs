using BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HealthMonitoring.Auth
{
    public class UserAccess : AuthorizationFilterAttribute
    { 
    public override void OnAuthorization(HttpActionContext actionContext)
    {
        var auth = actionContext.Request.Headers.Authorization;
        if (auth == null)
        {
            actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "No token Supplied");
        }
        else if (!AuthService.IsTokenValidUser(auth.ToString()))
        {
            actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "Supplied token is expired/invalid");
        }
        base.OnAuthorization(actionContext);
    }
}
}
