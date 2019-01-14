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


            //services
            //    .AddHealthChecks()
            //    .AddCheck(new SqlConnectionHealthCheck("MyDatabase", Configuration["ConnectionStrings:DefaultConnection"]));

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

            ;

            //    .AddUrlGroup(new Uri("https://api.simplecast.com/v1/podcasts.json"), "Simplecast API", HealthStatus.Degraded)

            //.AddSqlServer(
            //    connectionString: Configuration["Data:ConnectionStrings:Sql"],
            //    healthQuery: "SELECT 1;",
            //    name: "sql",
            //    failureStatus: HealthStatus.Degraded,
            //    tags: new string[] { "db", "sql", "sqlserver" });

            //services.AddSingleton<SomeDependency>();


            // register the custom health check 
            // after AddHealthChecks and after SomeDependency 
            //  services.AddSingleton<IHealthCheck, SomeHealthCheck>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //  app.UseHealthChecks("/healthz");

            app.UseHealthChecksUI(option => { option.UIPath = "/hz"; });


            //app.UseHealthChecks("/hc",
            //    new HealthCheckOptions
            //    {
            //        ResponseWriter = async (context, report) =>
            //        {
            //            var result = JsonConvert.SerializeObject(
            //                new
            //                {
            //                    status = report.Status.ToString(),
            //                    errors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
            //                });
            //            context.Response.ContentType = MediaTypeNames.Application.Json;
            //            await context.Response.WriteAsync(result);
            //        }
            //    });


            app.UseMvc();
        }
    }
}
