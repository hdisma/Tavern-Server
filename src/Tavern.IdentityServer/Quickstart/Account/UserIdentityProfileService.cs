using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tavern.IdentityServer.DbContexts;
using Tavern.IdentityServer.Models;

namespace Tavern.IdentityServer.Quickstart.Account
{
    public class UserIdentityProfileService : IProfileService
    {
        private readonly TavernUsersDbContext _tavernUsersDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private List<Claim> _claims;

        public UserIdentityProfileService(
                                    TavernUsersDbContext tavernUsersDbContext,
                                    UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager)
        {
            _tavernUsersDbContext = tavernUsersDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _claims = new List<Claim>();
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            var userRoles = await _tavernUsersDbContext.UserRoles.Where(r => r.UserId == user.Id).ToListAsync();

            // Get RoleClaims for the current user as permissions claimType
            await GetRoleClaims(userRoles);
            // Get UserClaims for the current user as permissions claimType
            await GetUserClaims(user);
            // Remove duplicated claims types.
            _claims = _claims.Distinct(new ClaimEqualityComparer()).ToList();
            // Transform user roles to claim type roles
            await TransformUserRolesToClaims(userRoles);

            _claims.Add(new Claim("username", user.UserName));

            context.IssuedClaims = _claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(subjectId);

            context.IsActive = user != null;
        }

        private async Task GetRoleClaims(List<IdentityUserRole<string>> userRoles)
        {
            foreach (var role in userRoles)
            {
                var result = await _tavernUsersDbContext.RoleClaims.Where(rc => 
                                                                            rc.RoleId == role.RoleId && rc.ClaimValue == true.ToString())
                                                                   .ToListAsync();
                result.ForEach(claim => _claims.Add(new Claim("permissions", claim.ClaimType)));
            }
        }

        private async Task GetUserClaims(ApplicationUser user)
        {
            var userClaims = await _tavernUsersDbContext.UserClaims.Where(c => 
                                                                            c.UserId == user.Id && c.ClaimValue == true.ToString())
                                                                   .ToListAsync();
            foreach (var claim in userClaims)
                _claims.Add(new Claim("permissions", claim.ClaimType));
        }
    
        private async Task TransformUserRolesToClaims(List<IdentityUserRole<string>> userRoles)
        {
            foreach (var role in userRoles)
                _claims.Add(new Claim("roles", (await _roleManager.FindByIdAsync(role.RoleId)).Name));
        }
    }

    public class ClaimEqualityComparer : IEqualityComparer<Claim>
    {
        public bool Equals(Claim x, Claim y)
        {
            return x.Value.Equals(y.Value);
        }

        public int GetHashCode([DisallowNull] Claim obj)
        {
            return obj.Value.GetHashCode();
        }
    }
}
