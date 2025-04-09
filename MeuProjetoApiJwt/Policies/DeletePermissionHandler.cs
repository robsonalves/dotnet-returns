using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

public class DeletePermissionHandler : AuthorizationHandler<DeletePermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DeletePermissionRequirement requirement)
    {
        var canDeleteClaim = context.User.FindFirst("canDelete");
        var teamClaim = context.User.FindFirst("team");
        var roleClaim = context.User.FindFirst(ClaimTypes.Role);

        Console.WriteLine("🔐 [Auth] Avaliando DeletePermissionHandler:");
        Console.WriteLine($"canDelete: {canDeleteClaim?.Value}");
        Console.WriteLine($"team: {teamClaim?.Value}");
        Console.WriteLine($"role: {roleClaim?.Value}");

        if (canDeleteClaim is not null && canDeleteClaim.Value == "true")
        {
            if (!string.IsNullOrEmpty(requirement.RequiredTeam))
            {
                if (teamClaim is not null &&
                    teamClaim.Value.Equals(requirement.RequiredTeam, StringComparison.OrdinalIgnoreCase))
                {
                    context.Succeed(requirement);
                    Console.WriteLine("✅ Autorizado: canDelete=true e team coincide.");
                }
                else
                {
                    Console.WriteLine("❌ Bloqueado: team não coincide.");
                }
            }
            else
            {
                context.Succeed(requirement);
                Console.WriteLine("✅ Autorizado: canDelete=true sem exigência de team.");
            }
        }
        else if (roleClaim?.Value == "admin")
        {
            context.Succeed(requirement);
            Console.WriteLine("✅ Autorizado: fallback via role=admin.");
        }
        else
        {
            Console.WriteLine("❌ Bloqueado: Nenhuma condição atendida.");
        }

        return Task.CompletedTask;
    }
}