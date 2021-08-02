using DbAccess;
using DbAccess.Repositories;
using DbAccess.RepositoryInterfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DataManagement.Api
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

            services.AddControllers();
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContext<BackEyeContext>(opt => opt.UseMySQL(connectionString).EnableSensitiveDataLogging());
            services.AddDbContext<BackEyeContext>(opt => opt.UseSqlServer(connectionString));
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ILogRepository, LogsRepository>();
            services.AddScoped<IMeasurementRepository, MeasurementRepository>();
            services.AddScoped<IStudentLessonRepository, StudentLessonRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DataManagement.Api", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins(Configuration.GetSection("ClientUrl").Value));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\DataManagement.Api.log.txt");
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DataManagement.Api v1");
                    c.RoutePrefix = string.Empty;
                    });
            //}


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<StreamHub>("/streamHub");
            });
            app.UseCors("CorsPolicy");
        }
    }
}
