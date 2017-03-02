using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Security;

namespace SeguridadWebv2.Helpers
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            // check  sessions here
            //if (HttpContext.Current.Session["username"] == null)
            //{
            //    filterContext.Result = new RedirectResult("~/Home/Dashboard");
            //    return;
            //}

            // check if session is supported
            if (ctx.Session != null)
            {
                // check if a new session id was generated
                if (ctx.Session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must
                    // have timed out
                    string sessionCookie = ctx.Request.Headers["Cookie"];
                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        string redirectOnSuccess = filterContext.HttpContext.Request.Url.PathAndQuery;
                        string redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
                        string loginUrl = FormsAuthentication.LoginUrl + redirectUrl;
                        if (ctx.Request.IsAuthenticated)
                        {
                            FormsAuthentication.SignOut();
                        }
                        RedirectResult rr = new RedirectResult(loginUrl);
                        filterContext.Result = rr;
                        //ctx.Response.Redirect("~/Home/Logon");

                    }
                }
            }

            //if (HttpContext.Current.Session.Timeout == 2)
            //{
            //    filterContext.Result = new RedirectResult("~/VideoConferencia/ValidarConsulta");
            //    return;
            //}
            base.OnActionExecuting(filterContext);
        }
    }
}