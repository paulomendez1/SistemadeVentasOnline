using SistemadeVentasOnline.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SistemadeVentasOnline.Filters
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            SistemadeVentasEntities db = new SistemadeVentasEntities();
            string Email = Convert.ToString(System.Web.HttpContext.Current.Session["User"]);
            int Rol = Convert.ToInt32(System.Web.HttpContext.Current.Session["Rol"]);
            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string tag = controllerName;

            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                // Don't check for authorization as AllowAnonymous filter is applied to the action or controller
                return;
            }

            // Check for authorization
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            if (Email != null && Email != "")
            {
                bool isPermitted = false;
                var viewPermission = db.Rols.Where(x => x.RolId == Rol&&x.Tag==tag).SingleOrDefault();
                if (viewPermission != null)
                {
                    isPermitted = true;
                }
                if (isPermitted == false)
                {
                    filterContext.Result = new RedirectToRouteResult(
                     new RouteValueDictionary
                       {
                             { "controller", "User" },
                             { "action", "AccessDenied" }
                       });
                }
            }
            }
    }
}