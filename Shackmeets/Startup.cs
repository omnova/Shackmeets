using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;

using Shackmeets.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

namespace Shackmeets
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
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      // Test connection
      var connection = @"Server=(localdb)\MSSQLLocalDB;Database=ShackmeetsDev;Trusted_Connection=True;ConnectRetryCount=0";

      services.AddDbContext<ShackmeetsDbContext>(options => options.UseSqlServer(connection));
      
      // In production, the React files will be served from this directory
      services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/build";
        });

      // http://jasonwatmore.com/post/2018/06/26/aspnet-core-21-simple-api-for-authentication-registration-and-user-management#startup-cs

      //  // Should retrieve from somewhere
      //  var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TEMP"));

      //  services.AddAuthentication(x =>
      //  {
      //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      //  })
      //  .AddJwtBearer(x =>
      //  {
      //    x.Events = new JwtBearerEvents
      //    {
      //      OnTokenValidated = context =>
      //      {
      //        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
      //        var userId = int.Parse(context.Principal.Identity.Name);
      //        var user = userService.GetById(userId);
      //        if (user == null)
      //        {
      //                // return unauthorized if user no longer exists
      //                context.Fail("Unauthorized");
      //        }

      //        return Task.CompletedTask;
      //      }
      //    };
      //    x.RequireHttpsMetadata = false;
      //    x.SaveToken = true;
      //    x.TokenValidationParameters = new TokenValidationParameters
      //    {
      //      ValidateIssuerSigningKey = true,
      //      IssuerSigningKey = new SymmetricSecurityKey(key),
      //      ValidateIssuer = false,
      //      ValidateAudience = false
      //    };
      //  }); ns.Audience = "shackmeets.com";
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
      app.UseSpaStaticFiles();
      //app.UseAuthentication();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
          name: "default",
          template: "{controller}/{action=Index}/{id?}");
      });

      app.UseSpa(spa =>
      {
        spa.Options.SourcePath = "ClientApp";

        if (env.IsDevelopment())
        {
          spa.UseReactDevelopmentServer(npmScript: "start");
        }
      });
    }
  }
}
