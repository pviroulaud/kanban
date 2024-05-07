using Microsoft.AspNetCore.Mvc.Filters;
using serviciosKanban.SRVC;

namespace Kanban.Filters
{
    public class informacionToken :ActionFilterAttribute,  IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var JWT = new JwtSrvc();
            var infoToken = JWT.getInformacionToken(context.HttpContext.User.Identity);
            if (infoToken.ContainsKey("Id"))
            {
                context.HttpContext.Items.Add("idUsuarioToken", Convert.ToInt32(infoToken["Id"]));
            }
        }
    }
}
