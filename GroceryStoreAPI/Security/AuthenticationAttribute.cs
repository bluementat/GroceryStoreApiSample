using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace GroceryStoreAPI.Security
{
    internal class AuthenticationAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IConfiguration config = (IConfiguration)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));
            ITokenService tokenService = (ITokenService)context.HttpContext.RequestServices.GetService(typeof(ITokenService));

            var token = context.HttpContext.Request.Headers["Token"].ToString();

            if (tokenService.IsTokenValid(config["Jwt:Key"].ToString(), config["Jwt:Issuer"].ToString(), token))
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}