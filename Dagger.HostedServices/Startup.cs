using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dagger.Data.Connections;
using Dagger.Data.DTOs;
using Dagger.Data.Queues;
using Dagger.Data.Repositories;
using Dagger.Digraph;
using Dagger.HostedServices.Application.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dagger.HostedServices
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //create configuration
            var builder = new ConfigurationBuilder().SetBasePath(System.IO.Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfiguration config = builder.Build();

            string postgresqlConnectionString = config.GetSection("ConnectionStrings")["PolicyGuardianConnectionString"];

            //create connection 
            IConnection connection = new Connection(postgresqlConnectionString);

            //configure services
            services.AddSingleton<IConnection>(connection);
            services.AddSingleton<IHealthCheck, HealthCheck>();
            services.AddTransient<IRepository<ConfigurationDTO>, Repository<ConfigurationDTO>>();
            services.AddTransient<IConsumerFactory, ConsumerFactory>();
            services.AddSingleton<IManager, Manager>();
            services.AddSingleton<HostedServices.HostedService>();

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
