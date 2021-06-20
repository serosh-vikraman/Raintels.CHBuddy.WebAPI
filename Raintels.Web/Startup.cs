using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Raintels.Core.DataManager;
using Raintels.Core.Interface;
using Raintels.Entity.ViewModel;
using Raintels.Service;
using Raintels.Service.Service;
using Raintels.Service.ServiceInterface;
using System.IO;

namespace Raintels.CHBuddy.Web.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }
        public IWebHostEnvironment HostingEnvironment { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();

            services.AddSpaStaticFiles(config =>
            {
                config.RootPath = "wwwroot";
            });

            services.AddControllers();

            var pathToKey = Path.Combine
           (Directory.GetCurrentDirectory(), "keys", "firebase_admin_sdk.json");

            if (HostingEnvironment.IsEnvironment("local"))
                pathToKey = Path.Combine(Directory.GetCurrentDirectory(),
                            "keys", "firebase_admin_sdk.local.json");

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(pathToKey)
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var firebaseProjectName = Configuration["FirebaseProjectName"];
                    options.Authority =
                    "https://securetoken.google.com/" + firebaseProjectName;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "https://securetoken.google.com/" + firebaseProjectName,
                        ValidateAudience = true,
                        ValidAudience = firebaseProjectName,
                        ValidateLifetime = true
                    };
                });

            services.AddAutoMapper(typeof(AutoMapping));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudentManager, StudentManager>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEventManager, EventManager>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            //app.UseAuthorization();
            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
             //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = "client-app";
            //    if (env.IsDevelopment() || env.IsEnvironment("local"))
            //    {
            //        var startScript = env.IsEnvironment("local") ? "start-local" : "start";
            //        spa.UseAngularCliServer(npmScript: startScript);
            //    }
            //});
        }
    }
}
