using Api.Entities.Interfaces;
using Api.Extentions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Entities.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (!resultContext.HttpContext.User.Identity.IsAuthenticated)
            return;
            var x= resultContext.HttpContext.User.GetUserId();
            // getting current logged-in user name
            Guid userId = new();


            //Save the last activity time into user.LastActive
            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepo>();
            var user = await repo.GetUserByIdAsync(userId);

            user.LastActive = DateTime.UtcNow;
            await repo.SaveAllAsync();
        }
    }
}