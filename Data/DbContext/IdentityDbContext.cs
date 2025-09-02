using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.DbContext
{
    public class IdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }
        public DbSet<TwoFaToken> TwoFaTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<TwoFaToken>(entity =>
            {
                entity.HasKey(t => t.Id); 
                entity.Property(t => t.Token).IsRequired();
                entity.Property(t => t.ExpiryDate).IsRequired();
            });
        }
    }
}
