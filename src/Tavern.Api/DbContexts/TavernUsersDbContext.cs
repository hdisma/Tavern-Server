using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tavern.Api.Entities;

namespace Tavern.Api.DbContexts
{
    public class TavernUsersDbContext : IdentityDbContext<ApplicationUser>
    {
        public TavernUsersDbContext(DbContextOptions<TavernUsersDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
