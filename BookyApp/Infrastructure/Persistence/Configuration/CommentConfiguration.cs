using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Persistence.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            

            

            builder.HasOne(x => x.quotation)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.QuotationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.user)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            //builder.HasMany(x => x.ArticleTags)
            //    .WithOne(x => x.Article)
            //    .HasForeignKey(x => x.ArticleId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(x => x.Files)
            //    .WithOne(x => x.Article)
            //    .HasForeignKey(x => x.ArticleId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
