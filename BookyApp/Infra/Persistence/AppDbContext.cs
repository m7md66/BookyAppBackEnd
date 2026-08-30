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
        //public AppDbContext() 
        //{

        //}
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
        public DbSet<UserInterest>? UserInterests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo().EnableSensitiveDataLogging().EnableDetailedErrors();
            //optionsBuilder.EnableDetailedErrors();
            //optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            Seed(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


        private void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genres>().HasData(
                new Genres { Id = new Guid("4e5a43d7-dafd-49fd-b423-16defbb56726"), Name = "Literary Fiction", NameAr = "خيال أدبي" },
                new Genres { Id = new Guid("4e5a44d7-dafd-49fd-b423-16defbb56726"), Name = "LGBTQ+", NameAr = "مجتمع الميم" },
                new Genres { Id = new Guid("4e5a41d7-dafd-49fd-b423-16defbb56726"), Name = "Contemporary Fiction", NameAr = "روايات معاصرة" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56726"), Name = "Romance", NameAr = "رومانسي" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56725"), Name = "Historical Fiction", NameAr = "خيال تاريخي" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56724"), Name = "Thriller & Suspense", NameAr = "إثارة وتشويق" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56723"), Name = "Horror", NameAr = "رعب" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56722"), Name = "Mystery", NameAr = "غموض" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56721"), Name = "Action & Adventure", NameAr = "أكشن ومغامرة" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56720"), Name = "Dystopian", NameAr = "ديستوبيا" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56736"), Name = "Science Fiction", NameAr = "خيال علمي" },
                new Genres { Id = new Guid("4e5a41d9-dafd-49fd-b423-16defbb56746"), Name = "Fantasy", NameAr = "فانتازيا" },
                new Genres { Id = new Guid("963b0835-e87a-44ed-a9b4-f9488e481a96"), Name = "Crime", NameAr = "جريمة" },
                new Genres { Id = new Guid("ed620332-d5bf-4a42-8036-135bf49da63f"), Name = "Drama", NameAr = "دراما" },
                new Genres { Id = new Guid("5df1e3a4-b736-4987-87eb-ea7474f81016"), Name = "Poetry", NameAr = "شعر" },
                new Genres { Id = new Guid("419727ee-9e34-42da-a231-fcf748a1d0a1"), Name = "Biography & Memoir", NameAr = "سيرة ومذكرات" },
                new Genres { Id = new Guid("71f9c4aa-1c28-4508-a98b-8c77cd0480ed"), Name = "Self-Help", NameAr = "تطوير الذات" },
                new Genres { Id = new Guid("9cbe915e-6092-4f04-8055-bdc787b6ba5d"), Name = "Philosophy", NameAr = "فلسفة" },
                new Genres { Id = new Guid("10c7fa4b-31c5-4004-83b8-a51ef4a8c462"), Name = "Psychology", NameAr = "علم نفس" },
                new Genres { Id = new Guid("ba0ffbb1-0509-4e1c-8d40-15c605b37832"), Name = "History", NameAr = "تاريخ" },
                new Genres { Id = new Guid("12284e84-3f74-4f66-89db-1f31afa378cf"), Name = "Business", NameAr = "أعمال" },
                new Genres { Id = new Guid("aef82253-4f0c-4fda-96f0-0ce57c48ab68"), Name = "Science & Nature", NameAr = "علوم وطبيعة" },
                new Genres { Id = new Guid("925403f3-0ad5-455c-9962-6e63572c9cfd"), Name = "Young Adult", NameAr = "يافعين" },
                new Genres { Id = new Guid("df685395-72d2-4156-83d5-5307b1fc5bf9"), Name = "Classics", NameAr = "كلاسيكيات" },
                new Genres { Id = new Guid("ce3b2e9d-e813-4318-aa97-bf4542e7e3d4"), Name = "Graphic Novels & Comics", NameAr = "روايات مصوّرة وكوميكس" }
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
