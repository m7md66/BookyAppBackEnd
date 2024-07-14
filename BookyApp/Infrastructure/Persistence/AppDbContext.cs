using Domain.Common;
using Domain.Entities;
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

namespace Infrastructure.Persistence
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {

        public AppDbContext() 
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Book>? Books { get; set; }
        public DbSet<FavoriteUserBooks>? favoriteUserBooks { get; set; }
        //public DbSet<FavoriteBook>? FavoriteBooks { get; set; }
        public DbSet<Quotation>? Quotations { get; set; }
        public DbSet<QuotationLike>? QuotationLikes { get; set; }
        public DbSet<QuotationShare>? QuotationShares { get; set; }
        public DbSet<ReQuote>? ReQuotes { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Interest>? Interests { get; set; }
        public DbSet<UserInterests>? UserInterests { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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
                        //entry.Entity.CreatedBy = _session.UserId;
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
