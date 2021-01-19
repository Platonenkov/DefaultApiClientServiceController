using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ApiServer.Test;
using ApiServer.Test.Service;
using DefaultApiClientService.Client;
using Domain;
using Infrastucture;
using Infrastucture.Context;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Newtonsoft.Json;

namespace ApiServer
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiServer", Version = "v1" });
            });


            //ToDo When Use it, don't forget add-migration and update-database
            services.AddTransient<TestContextInitializer>();
            services.AddDbContext<TestContext>(o =>
            {
                o.UseLazyLoadingProxies();
                o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<HttpClient>();

            services.AddScoped<IStudentService, StudentService>();
            services.AddSingleton<BaseApiResponse<Student>>();


            services.AddOData();
            services.AddMvc().AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.Formatting = Formatting.Indented;
                })
               .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.WriteIndented = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TestContextInitializer context)
        {
            context.InitializeAsync().Wait();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiServer v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapODataRoute(
                    routeName: "api",
                    routePrefix: "api",
                    model: GetEdmModel());

                endpoints.EnableDependencyInjection();
                endpoints.Expand().Select().Count().Filter().OrderBy().SkipToken().MaxTop(1);
                endpoints.MapControllers();
            });
        }

        private IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Student>("Students");
            return builder.GetEdmModel();
        }
    }
}
