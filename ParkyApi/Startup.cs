using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParkyApi.Data;
using ParkyApi.Mapper;
using ParkyApi.Repository;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ParkyApi
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

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("constr"))
            ) ;

            services.AddAutoMapper(typeof(ParkyMappings));
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();


            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("parkyOpenApi", new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "parky api",
                        Version = "1",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Email = "Ahmed.Hamdy2061995@gmail.com",
                            Name = "Ahmed Hamdy"
                        },
                        Description = "Parky Api Documentation",
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name = "Ahmed Hamdy Licence"
                        }

                    }) ;

                    // if you need to run comments in swagger
                    //var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name }.xml";
                    //var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                    //options.IncludeXmlComments(xmlCommentsFullPath);
                }
                );


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(option=> {

                option.SwaggerEndpoint("/swagger/parkyOpenApi/swagger.json", "parky api");
                option.RoutePrefix = "";

            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
