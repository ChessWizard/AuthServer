using AuthServer.Core.Entities;
using AuthServer.Core.Enums;
using AuthServer.Data.Context;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data.Seeds
{
    public static class RoleSeeds
    {
        public static async Task AddSeedRoles(AuthServerDbContext context)
        {
            var roles = await context.Roles
                .AsNoTracking()
                .ToListAsync();

            if (roles.IsNullOrNotAny())
            {
                List<Role> addingRoles = new()
                {
                    new() { RoleType = RoleType.Admin, Title = "Admin Rolü", Name = "Admin", NormalizedName = "ADMIN"},
                    new() { RoleType = RoleType.Corporate, Title = "Kurumsal Kullanıcı Rolü", Name = "Corporate", NormalizedName = "CORPORATE"},
                    new() { RoleType = RoleType.Individual, Title = "Bireysel Kullanıcı Rolü", Name = "Individual", NormalizedName = "INDIVIDUAL"}
                };
                await context.AddRangeAsync(addingRoles);
                await context.SaveChangesAsync();
            }
        }
    }
}
