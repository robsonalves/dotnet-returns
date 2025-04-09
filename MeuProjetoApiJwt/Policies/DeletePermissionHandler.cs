using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class DeletePermissionHandler : AuthorizationHandler<DeletePermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DeletePermissionRequirement requirement)
    {
        var canDeleteClaim = context.User.FindFirst("canDelete");
        var teamClaim = context.User.FindFirst("team");
        var roleClaim = context.User.FindFirst(ClaimTypes.Role);

        if (canDeleteClaim is not null && canDeleteClaim.Value == "true")
        {
            if (!string.IsNullOrEmpty(requirement.RequiredTeam))
            {
                if (teamClaim is not null &&
                    teamClaim.Value.Equals(requirement.RequiredTeam, StringComparison.OrdinalIgnoreCase))
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                context.Succeed(requirement);
            }
        }
        else if (roleClaim?.Value == "admin")
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}