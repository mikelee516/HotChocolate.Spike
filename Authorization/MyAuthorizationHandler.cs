using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace HotChocolate.Spike.Authorization
{
    public class MyAuthorizationHandler : AuthorizationHandler<MyAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public MyAuthorizationHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MyAuthorizationRequirement requirement)
        {
            if (requirement.PolicyName == "Pass")
            {
                context.Succeed(requirement);
            }
        }

    }
}