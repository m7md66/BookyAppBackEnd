using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Persistence.Configuration
{
    public class FavoriteUserBooksConfiguration : IEntityTypeConfiguration<FavoriteUserBooks>
    {


        public void Configure(EntityTypeBuilder<FavoriteUserBooks> builder)
        {
            builder.HasIndex(b => new { b.BookId, b.UserId })
              .IsUnique();
      
        }
    }
}
