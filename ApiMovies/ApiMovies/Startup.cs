using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ApiMovies.Data;
using ApiMovies.Interface.IGenericRepository;
using ApiMovies.Mapper;
using ApiMovies.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ApiMovies
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            /*Generate Token*/
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };

                    });


            //Add AutoMapper
            services.AddAutoMapper(typeof(AutoMappers));

   

            /*Documentation*/
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiMovies", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Api Movies",
                    Version = "v1",

                });


                //File Comments Documentation
                //var FileCommentsDocumentation = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var PathFileCommentsDocumentation = Path.Combine(AppContext.BaseDirectory, "ApiMoviesDocumentation.xml");
                options.IncludeXmlComments(PathFileCommentsDocumentation);

            });


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiCategorias", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Api Movies",
                    Version = "v1",

                });


                //File Comments Documentation
                //var FileCommentsDocumentation = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var PathFileCommentsDocumentation = Path.Combine(AppContext.BaseDirectory, "ApiMoviesDocumentation.xml");
                options.IncludeXmlComments(PathFileCommentsDocumentation);

            });


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiUsuarios", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Api Movies",
                    Version = "v1",

                });


                //File Comments Documentation
                //var FileCommentsDocumentation = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var PathFileCommentsDocumentation = Path.Combine(AppContext.BaseDirectory, "ApiMoviesDocumentation.xml");
                options.IncludeXmlComments(PathFileCommentsDocumentation);

            });
            /*End Documentation*/

            services.AddControllers();

            /*Soporte para CORS*/
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }

            /*Documentation*/
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/ApiMovies/swagger.json", "Api Movies");
                options.SwaggerEndpoint("/swagger/ApiCategorias/swagger.json", "Api Categorias");
                options.SwaggerEndpoint("/swagger/ApiUsuarios/swagger.json", "Api Usuarios");
                options.RoutePrefix = "";
            });
            /*End Documentation*/

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            /*Soporte para CORS*/
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}
