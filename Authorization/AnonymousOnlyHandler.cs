using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SpaAndBeautyWebsite.Authorization
{
    public class AnonymousOnlyHandler : AuthorizationHandler<AnonymousOnlyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AnonymousOnlyRequirement requirement)
        {
            // Succeed only if the user is NOT authenticated
            if (!(context.User?.Identity?.IsAuthenticated ?? false))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}