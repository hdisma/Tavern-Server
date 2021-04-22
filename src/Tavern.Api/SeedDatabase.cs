using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tavern.Api.DbContexts;
using Tavern.Api.Entities;

namespace Tavern.Api
{
    public class SeedDatabase
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            var tavernDbContext = serviceProvider.GetRequiredService<TavernDbContext>();
            var tavernUsersDbContext = serviceProvider.GetRequiredService<TavernUsersDbContext>();

            IUserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(tavernUsersDbContext);
            IRoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(tavernUsersDbContext);

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore, null, null, null,
                                                                                        null, null, null, null, null);
            RoleManager<IdentityRole> roleManager = new(roleStore, null, null, null, null);

            var users = userManager.Users.ToList();
            var roles = roleManager.Roles.ToList();

            tavernDbContext.Database.EnsureCreated();
            tavernUsersDbContext.Database.EnsureCreated();

            if (!tavernDbContext.Categories.Any())
            {
                var category = new Category()
                {
                    Name = "Beer",
                    Description = "Very Cold Beers :D",
                };

                tavernDbContext.Categories.Add(category);
                tavernDbContext.SaveChanges();
            }

            if (!tavernDbContext.Products.Any())
            {
                var category = tavernDbContext.Categories.FirstOrDefault();

                var product = new Product()
                {
                    Name = "Heineken",
                    Description = "A very cold Heineken :D",
                    CategoryId = category.Id
                };

                tavernDbContext.Products.Add(product);
                tavernDbContext.SaveChanges();
            }

            if (!tavernUsersDbContext.Users.Any())
            {
                ApplicationUser user = new()
                {
                    Email = "test@test.com",
                    UserName = "test",
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                await userManager.CreateAsync(user, "Test@123");
            }

            if (!tavernUsersDbContext.Roles.Any())
            {
                var adminRole = new IdentityRole()
                {
                    Name = "Admin"
                };

                var userRole = new IdentityRole()
                {
                    Name = "User"
                };

                await roleManager.CreateAsync(adminRole);
                await roleManager.CreateAsync(userRole);
            }

            if (!tavernUsersDbContext.RoleClaims.Any())
            {
                var adminRole = roleManager.Roles.FirstOrDefault(x => x.Name == "Admin");
                var userRole = roleManager.Roles.FirstOrDefault(x => x.Name == "User");
                await roleManager.AddClaimAsync(adminRole, new Claim("canAccessCategories", "true"));
                await roleManager.AddClaimAsync(userRole, new Claim("canAccessProducts", "true"));
            }

            if (!tavernUsersDbContext.UserRoles.Any())
            {
                var user = tavernUsersDbContext.Users.FirstOrDefault();
                var addProductPermission = new Claim("canAddProducts", "true");

                await userManager.AddClaimAsync(user, addProductPermission);
            }

        }
    }
}
