using System;
using Api;
using api.Handler;
using api.Migrations;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace api
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
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            var connectionDefault = Configuration.GetConnectionString("CONNECTION_DEFAULT");

            services.AddControllers();
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<InsuranceContext>(opt =>
                    {
                        opt.UseNpgsql(connectionString ?? connectionDefault);
                        opt.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
                    });
            
            services.AddHangfire(x => x.UsePostgreSqlStorage(connectionString?? connectionDefault));
            services.AddHangfireServer();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
            });

            services.AddTransient<IInsuranceRepository, InsuranceRepository>();
            services.AddScoped<IInsurance, Insurance>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            RecurringJob.AddOrUpdate<IInsurance>(
                "ScanAllFraudProfiles",
                x => x.ScanAllFraudProfile(),
                "0 17 * * *");
        }
    }
}
