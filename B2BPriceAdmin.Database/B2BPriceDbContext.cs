using B2BPriceAdmin.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace B2BPriceAdmin.Database
{
    public class B2BPriceDbContext : IdentityDbContext<User, Role, int>
    {
        public B2BPriceDbContext()
        {
        }

        public B2BPriceDbContext(DbContextOptions options) : base(options)
        {

        }

        #region DbSet
        public virtual DbSet<Tenant> Tenants { get; set; }

        #endregion

        #region DBConfiguration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Local

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\;Database=B2BPRICEDB;user id=sa;password=open;", x => x.MigrationsHistoryTable("__B2BPRICEADMINMigrationsHistory"));
            }

        }
        #endregion

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseTenantEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        //user id here
                        entry.Entity.CreatedBy = 1;
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = 1;
                        entry.Entity.LastModifiedOn = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Identity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Identity.User");
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable(name: "Identity.Role");
            });
            modelBuilder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("Identity.UserRoles");
            });
            modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("Identity.UserClaims");
            });
            modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("Identity.UserLogins");
            });
            modelBuilder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("Identity.RoleClaims");
            });
            modelBuilder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("Identity.UserTokens");
            });
            #endregion

        }
    }
}
