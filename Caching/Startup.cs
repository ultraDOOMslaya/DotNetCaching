using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caching.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Caching
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

            services.AddMemoryCache();

            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase(databaseName: "Drugs"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "Drugs";
            });
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //AddTestData(context);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=DistDrug}/{action=Index}/{id?}");
            });
        }

        private static void AddTestData(ApiContext context)
        {
            var drug1 = new Models.Drug
            {
                Id = 1,
                drugNdc = "000918370",
                drugName = "Lipitor",
                drugPrice = Decimal.Parse("87.99", System.Globalization.NumberStyles.Currency),
                packSize = 30
            };

            context.Drugs.Add(drug1);

            var drug2 = new Models.Drug
            {
                Id = 2,
                drugNdc = "000914570",
                drugName = "Viagra",
                drugPrice = Decimal.Parse("34.99", System.Globalization.NumberStyles.Currency),
                packSize = 10
            };

            context.Drugs.Add(drug2);

            context.SaveChanges();
        }
    }
}
