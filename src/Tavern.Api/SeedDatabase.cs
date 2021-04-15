using Microsoft.AspNetCore.Identity;
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
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

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
                var role = new IdentityRole()
                {
                    Name = "Admin"
                };

                await roleManager.CreateAsync(role);
            }

            if (!tavernUsersDbContext.RoleClaims.Any())
            {
                var role = roleManager.Roles.FirstOrDefault(x => x.Name == "Admin");
                await roleManager.AddClaimAsync(role, new Claim("canAccessCategories", "true"));
            }

            if (!tavernUsersDbContext.UserRoles.Any())
            {
                var user = tavernUsersDbContext.Users.FirstOrDefault();

                await userManager.AddToRoleAsync(user, "Admin");
            }

            //var roleUser = new IdentityRole()
            //{
            //    Name = "User"
            //};

            //await roleManager.CreateAsync(roleUser);

            //var existingRole = roleManager.Roles.FirstOrDefault(x => x.Name == "User");
            //await roleManager.AddClaimAsync(existingRole, new Claim("canAccessCategories", "true"));


            //var ExistingUser = tavernUsersDbContext.Users.FirstOrDefault();

            //await userManager.AddToRoleAsync(ExistingUser, "User");

            //var existingUser = tavernUsersDbContext.Users.FirstOrDefault();

            //var claim = new Claim("test", "testvalue");

            //await userManager.AddClaimAsync(existingUser, claim);
        }
    }
}
