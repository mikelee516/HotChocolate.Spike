using Microsoft.AspNetCore.Authorization;

namespace HotChocolate.Spike.Authorization
{
    public class MyAuthorizationRequirement : IAuthorizationRequirement
    {
        public MyAuthorizationRequirement(string policyName)
        {
            PolicyName = policyName;
        }

        public string PolicyName { get; }
    }
}