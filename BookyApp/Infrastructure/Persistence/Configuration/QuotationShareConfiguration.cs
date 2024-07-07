using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configuration
{
    public class QuotationShareConfiguration : IEntityTypeConfiguration<QuotationShare>
    {
        public void Configure(EntityTypeBuilder<QuotationShare> builder)
        {




            builder.HasOne(x => x.Quotation)
                .WithMany(x => x.QuotationShares)
                .HasForeignKey(x => x.QuotationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
