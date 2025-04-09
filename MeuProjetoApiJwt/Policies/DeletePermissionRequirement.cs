using Microsoft.AspNetCore.Authorization;

public class DeletePermissionRequirement : IAuthorizationRequirement
{
    public string RequiredTeam { get; }
    public DeletePermissionRequirement(string requiredTeam)
    {
        RequiredTeam = requiredTeam;
    }
}