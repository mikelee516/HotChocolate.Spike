using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HotChocolate.Spike.Authorization
{
    public class MyPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly ILogger<MyPolicyProvider> _logger;
        private readonly AuthorizationOptions _options;

        public MyPolicyProvider(IOptions<AuthorizationOptions> options, ILogger<MyPolicyProvider> logger) : base(options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            _logger.LogInformation($"Getting policy for {policyName}");
            var policy = await base.GetPolicyAsync(policyName);

            if (policy == null)
            {
                policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new MyAuthorizationRequirement(policyName))
                    .Build();

                // strange error sometimes when the website calls into the API. Catch it here - borrowed from AM
                System.Diagnostics.Debug.Assert(policy != null);
                // Add policy to the AuthorizationOptions, so we don't have to re-create it each time
                _options.AddPolicy(policyName, policy);

            }
            return policy;
        }
    }
}