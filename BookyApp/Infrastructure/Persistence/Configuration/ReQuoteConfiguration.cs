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
    public class ReQuoteConfiguration : IEntityTypeConfiguration<ReQuote>
    {
        public void Configure(EntityTypeBuilder<ReQuote> builder)
        {
            builder.HasOne(x => x.Quotation)
    .WithMany(x => x.ReQuotes)
    .HasForeignKey(x => x.QuotationId)
    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
