using Domain.Entities; 
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Persistence.Configuration
{
    public class QuotationLikeConfiguration : IEntityTypeConfiguration<QuotationLike>
    {
        public void Configure(EntityTypeBuilder<QuotationLike> builder)
        {
            builder.HasOne(x => x.Quotation)
           .WithMany(x => x.QuotationLikes)
           .HasForeignKey(x => x.QuotationId)
           .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
