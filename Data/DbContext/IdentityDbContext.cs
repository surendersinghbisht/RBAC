using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Data.DbContext
{
    public class IdentityDbContext : IdentityDbContext<IdentityUser, ApplicationRole, string>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }
        public DbSet<TwoFaToken> TwoFaTokens { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Author { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<City> City { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Announcements> Announcements { get; set; }

        public DbSet<UserNotification> userNotifications { get; set; }

        public DbSet<UserTask> userTask { get; set; }

        public DbSet<RecentActivity> activity { get; set; }

           
        protected override void OnModelCreating(ModelBuilder builder)
        {

           

            base.OnModelCreating(builder);
            builder.Entity<ApplicationRole>(entity =>
            {
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.ShortDescription).HasMaxLength(500);
            });
            builder.Entity<UserTask>().ToTable("UserTasks");

            builder.Entity<UserTask>()
                .HasOne(t => t.AssignedBy)
                .WithMany()
                .HasForeignKey(t => t.AssignedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserTask>()
                .HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TwoFaToken>(entity =>
            {
                entity.HasKey(t => t.Id); 
                entity.Property(t => t.Token).IsRequired();
                entity.Property(t => t.ExpiryDate).IsRequired();
            });

            builder.Entity<Announcements>().
                HasOne(a => a.User).WithMany().HasForeignKey(a => a.CreatedBy);


            builder.Entity<Country>().HasData(
      new Country { Id = 1, Name = "India" },
      new Country { Id = 2, Name = "USA" },
      new Country { Id = 3, Name = "United Kingdom" }
  );

            builder.Entity<State>().HasData(
       new State { Id = 1, Name = "Uttarakhand", CountryId = 1 },
       new State { Id = 2, Name = "Delhi", CountryId = 1 },
       new State { Id = 3, Name = "Maharashtra", CountryId = 1 },

       new State { Id = 4, Name = "New York", CountryId = 2 },
       new State { Id = 5, Name = "California", CountryId = 2 },
       new State { Id = 6, Name = "Texas", CountryId = 2 },

       new State { Id = 7, Name = "England - London", CountryId = 3 },
       new State { Id = 8, Name = "England - Manchester", CountryId = 3 }
   );

            builder.Entity<City>().HasData(
      // India
      new City { Id = 1, Name = "Dehradun", StateId = 1 },
      new City { Id = 2, Name = "Nainital", StateId = 1 },
      new City { Id = 3, Name = "New Delhi", StateId = 2 },
      new City { Id = 4, Name = "Mumbai", StateId = 3 },
      new City { Id = 5, Name = "Pune", StateId = 3 },

      // USA
      new City { Id = 6, Name = "New York City", StateId = 4 },
      new City { Id = 7, Name = "Buffalo", StateId = 4 },
      new City { Id = 8, Name = "Los Angeles", StateId = 5 },
      new City { Id = 9, Name = "San Francisco", StateId = 5 },
      new City { Id = 10, Name = "Houston", StateId = 6 },
      new City { Id = 11, Name = "Dallas", StateId = 6 },

      // UK
      new City { Id = 12, Name = "London", StateId = 7 },
      new City { Id = 13, Name = "Croydon", StateId = 7 },
      new City { Id = 14, Name = "Manchester", StateId = 8 },
      new City { Id = 15, Name = "Salford", StateId = 8 }
  );
        }
    }
}
