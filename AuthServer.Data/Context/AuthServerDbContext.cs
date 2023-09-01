using AuthServer.Core.Entities;
using AuthServer.Core.Entities.Common;
using AuthServer.Core.Entities.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data.Context
{
    public class AuthServerDbContext : IdentityDbContext<UserApp, Role, Guid>
    {
        public AuthServerDbContext(DbContextOptions<AuthServerDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Neighborhood> Neighborhoods { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<IdentityUserRole<Guid>> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(builder);
        }

        public class AuthServerDbContextFactory : IDesignTimeDbContextFactory<AuthServerDbContext>
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json", false)
            .Build();

            public AuthServerDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<AuthServerDbContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DbConnection"));

                return new AuthServerDbContext(optionsBuilder.Options);
            }
        }

        #region SaveChanges Interceptor

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            EnsureEntityType();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void EnsureEntityType()
        {
            var trackingEntities = ChangeTracker.Entries();

            foreach (var entity in trackingEntities)
            {
                if(entity.State is EntityState.Added && entity.Entity is IAuditEntity addedEntity)
                    addedEntity.CreatedDate = DateTimeOffset.UtcNow;

                if(entity.State is EntityState.Modified && entity.Entity is IAuditEntity modifiedEntity)
                    modifiedEntity.ModifiedDate = DateTimeOffset.UtcNow;
            }
        }

        #endregion
    }
}
