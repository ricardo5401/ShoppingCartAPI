using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using ShoppingCartApi.Models;
using ShoppingCartApi.Data;
using ShoppingCartApi.Extensions;
using ShoppingCartApi.Options;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL; 
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.Cors.Internal;

namespace ShoppingCartApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApiContext>(options => 
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().AddJsonOptions(a => a.SerializerSettings.ContractResolver = 
                    new CamelCasePropertyNamesContractResolver());
            services.AddCors();
            services.AddCors(options =>
            {
                /*options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://example.com"));*/
               options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                    });
            });

            services.Configure<TokenOptions>(Configuration.GetSection(nameof(TokenOptions)));

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "shoppingCart API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ApiContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingCartAPI V1");
            });

            DbInitializer.Initialize(context);

            var options = Configuration.GetSection(nameof(TokenOptions)).Get<TokenOptions>();

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters =
                {
                    ValidAudience = options.Audience,
                    ValidIssuer = options.Issuer,
                    IssuerSigningKey = options.GetSymmetricSecurityKey()
                }
            });

            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
