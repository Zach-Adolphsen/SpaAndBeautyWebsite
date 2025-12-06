using Microsoft.AspNetCore.Authorization;

namespace SpaAndBeautyWebsite.Authorization
{
    public sealed class AnonymousOnlyRequirement : IAuthorizationRequirement
    {
    }
}