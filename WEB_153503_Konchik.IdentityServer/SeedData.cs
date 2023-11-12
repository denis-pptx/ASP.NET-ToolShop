using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using WEB_153503_Konchik.IdentityServer.Data;
using WEB_153503_Konchik.IdentityServer.Models;

namespace WEB_153503_Konchik.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (roleMgr.FindByNameAsync("admin").Result is null)
                    roleMgr.CreateAsync(new IdentityRole("admin"));

                if (roleMgr.FindByNameAsync("user").Result is null)
                    roleMgr.CreateAsync(new IdentityRole("user"));


                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var user = userMgr.FindByNameAsync("user").Result;
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = "user",
                        Email = "user@gmail.com",
                        EmailConfirmed = true,
                    };

                    var result = userMgr.CreateAsync(user, "User123*").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddToRoleAsync(user, "user").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(user, new Claim[]{
                        new Claim(JwtClaimTypes.Name, user.UserName),
                        new Claim(JwtClaimTypes.Role, "user"),
                    }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    Log.Debug("user created");
                }
                else
                {
                    Log.Debug("user already exists");
                }

                var admin = userMgr.FindByNameAsync("admin").Result;
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@gmail.com",
                        EmailConfirmed = true
                    };

                    var result = userMgr.CreateAsync(admin, "Admin123*").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddToRoleAsync(admin, "admin").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(admin, new Claim[]{
                            new Claim(JwtClaimTypes.Name, admin.UserName),
                            new Claim(JwtClaimTypes.Role, "admin"),
                        }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    Log.Debug("admin created");
                }
                else
                {
                    Log.Debug("admin already exists");
                }
            }
        }
    }
}
