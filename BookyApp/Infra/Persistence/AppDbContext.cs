using Domain.Common;
using Domain.Entities;
using Infra.Helper.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Infra.Persistence
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        private readonly Session _session;
        public AppDbContext() 
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options, Session session) : base(options)
        {
            _session = session;
        }

        public DbSet<Book>? Books { get; set; }
        public DbSet<FavoriteUserBooks>? favoriteUserBooks { get; set; }
        //public DbSet<FavoriteBook>? FavoriteBooks { get; set; }
        public DbSet<Quotation>? Quotations { get; set; }
        public DbSet<QuotationLike>? QuotationLikes { get; set; }
        public DbSet<QuotationShare>? QuotationShares { get; set; }
        public DbSet<ReQuote>? ReQuotes { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Genres>? Genres { get; set; }
        public DbSet<UserInterests>? UserInterests { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            Seed(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


        private void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genres>().HasData(
                new Genres { Id = new Guid("4e5a43d7-dafd-49fd-b423-16defbb56726"), Name = "Literary Fiction" },
                new Genres { Id = new Guid("4e5a44d7-dafd-49fd-b423-16defbb56726"), Name = "LGBTQ+" },
                new Genres { Id = new Guid("4e5a41d7-dafd-49fd-b423-16defbb56726"), Name = "Contemporary Fiction" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56726"), Name = "Romance" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56725"), Name = "Historical Fiction" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56724"), Name = "Thriller & Suspense" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56723"), Name = "Horror" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56722"), Name = "Mystery" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56721"), Name = "Action & Adventure" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56720"), Name = "Dystopian" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56736"), Name = "Science Fiction" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56746"), Name = "Fantasy" },
                new Genres { Id =Guid.NewGuid(),  Name = "Fantasy" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56526"), Name = "Contemporary Fiction" }
            );

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });

            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "1",
                FirstName = "Admin",
                LastName = "Admin",
                PasswordHash = "AQAAAAIAAYagAAAAECUy6iEvdC2/0CconVvmgFiLOUUbKUfDDVr+nFHhNlap3uFO+aDhctUXjn06FPDL6Q==",//    aaa2222dddQQ

                SecurityStamp = "OPFOBYLCAKPCEUDBIA2UHRKLIMD2GGYT",
                Email = "admin@admin.com",
                ImageExtention = "admin@admin.com",
                ImageName = "admin@admin.com",
                ImageUrl = "admin@admin.com",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }); 

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "1",
                UserId = "1",
            });


        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Id = Guid.NewGuid();
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy =_session.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.UtcNow;
                        //entry.Entity.UpdatedBy = _session.UserId;
                        break;
                    default:
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
