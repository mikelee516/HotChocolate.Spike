using System.Collections.Generic;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.Spike.Authorization;
using HotChocolate.Spike.GraphQL;
using HotChocolate.Spike.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HotChocolate.Spike
{
    public class Startup
    {
        private const string AllowWildcardOrigins = "_allowWildcardOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(configureOptions =>
            {
                configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                configureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();

            services.AddCors(options =>
            {
                options.AddPolicy(AllowWildcardOrigins,
                    builder =>
                    {
                        builder
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .WithOrigins(Configuration.GetSection("CORS:AllowedOrigins").Get<List<string>>().ToArray())
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            services.AddTransient<WeatherForecastRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IAuthorizationPolicyProvider, MyPolicyProvider>();

            services.AddSingleton<IAuthorizationHandler, MyAuthorizationHandler>();

            services.AddControllers();

            // enable InMemory messaging services for subscription support.
            // services.AddInMemorySubscriptionProvider();

            // this enables you to use DataLoader in your resolvers.
            services.AddDataLoaderRegistry();

            // Add GraphQL Services
            services.AddGraphQL(serviceProvider =>
                SchemaBuilder.New()
                    .AddServices(serviceProvider)
                    .AddAuthorizeDirectiveType()
                    .AddQueryType<MyQueryType>()
                    .Create());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(AllowWildcardOrigins);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            // setting the path to differentiate
            app.UseGraphQL("/graphql");

            // setting the path to match 
            app.UsePlayground(new PlaygroundOptions()
            {
                Path = new PathString("/playground"),
                QueryPath = new PathString("/graphql")
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
