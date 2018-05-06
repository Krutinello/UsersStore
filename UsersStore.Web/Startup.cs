using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using UsersStore.Dal.Abstract;
using UsersStore.Dal.Concrete;
using UsersStore.Dal.EF;
using UsersStore.Web.TokenApp;

namespace UsersStore.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<UsersStoreContext>(options =>
                options.UseSqlServer(connection));

            //services.AddRouting();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthJwtTokenOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = AuthJwtTokenOptions.Audience,
                        ValidateLifetime = true,

                        IssuerSigningKey = AuthJwtTokenOptions.GetSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "User store", Description = "Swagger Core API" });
                c.IncludeXmlComments($"{AppDomain.CurrentDomain.BaseDirectory}UsersStore.Web.xml");
            });

            services.AddScoped<IUsersRepository, UsersRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core API"));

            //// определяем обработчик маршрута
            //var myRouteHandler = new RouteHandler(Handle);
            //// создаем маршрут, используя обработчик
            //var routeBuilder = new RouteBuilder(app, myRouteHandler);
            //// само определение маршрута - он должен соответствовать запросу {controller}/{action}
            //routeBuilder.MapRoute("default", "/");
            //// строим маршрут
            //app.UseRouter(routeBuilder.Build());




            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default_route",
            //        template: "{controller}/{action}/{id?}",
            //        defaults: new { controller = "Values", action = "Get" }
            //    );
            //});
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
        // собственно обработчик маршрута
        //private async Task Handle(HttpContext context)
        //{
        //    await context.Response.WriteAsync("Hello ASP.NET Core!");
        //}
    }
}
