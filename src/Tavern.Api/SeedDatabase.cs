using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tavern.Api.DbContexts;
using Tavern.Api.Entities;

namespace Tavern.Api
{
    public class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var tavernDbContext = serviceProvider.GetRequiredService<TavernDbContext>();
            var tavernUsersDbContext = serviceProvider.GetRequiredService<TavernUsersDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

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
                userManager.CreateAsync(user, "Test@123");
            }
        }
    }
}
