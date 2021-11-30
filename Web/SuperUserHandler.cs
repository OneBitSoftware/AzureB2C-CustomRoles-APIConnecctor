using Microsoft.AspNetCore.Authorization;

namespace ECS2021
{
    public class SuperUserHandler : AuthorizationHandler<SuperUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SuperUserRequirement requirement)
        {
            var superUserClaim = context.User.FindFirst(c => c.Type == "extension_role");

            if (superUserClaim != null && superUserClaim.Value.Equals(requirement.SuperUserValue))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
