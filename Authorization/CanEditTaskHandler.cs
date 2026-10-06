using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using securityapp.Models;

namespace securityapp.Authorization;

public class CanEditTaskHandler : AuthorizationHandler<CanEditTaskRequirement, TaskItem>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CanEditTaskRequirement requirement,
        TaskItem task)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (task.UserId == userId || context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}