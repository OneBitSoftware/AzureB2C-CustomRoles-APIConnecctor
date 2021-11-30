using Microsoft.AspNetCore.Authorization;

namespace ECS2021;

public class SuperUserRequirement : IAuthorizationRequirement
{
    public string SuperUserValue { get; } = "SuperUser";
}
