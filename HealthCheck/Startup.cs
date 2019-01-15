using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddHealthChecks()
                .AddOracle("")
                .AddMySql("")
                .AddSqlServer(
                    connectionString: Configuration["Data:ConnectionStrings:Sql"],
                    healthQuery: "SELECT 1;",
                    name: "sql",
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[] { "db", "s   ql", "sqlserver" })
                .AddRabbitMQ("");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseHealthChecksUI(option => { option.UIPath = "/hz"; });

            app.Use(async (context, next) =>
            {
                context.Response.Redirect("/hz");
            });
        }
    }
}
