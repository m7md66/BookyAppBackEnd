using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Quotation: EntityBase
    {
       public string UserId { get; set; }
        public Guid BookId { get; set; }
        public string Content { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
        [ForeignKey(nameof(BookId))]
        public virtual Book Book{ get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ReQuote> ReQuotes { get; set; }
        public virtual ICollection<QuotationShare> QuotationShares { get; set; }
        public virtual ICollection<QuotationLike> QuotationLikes { get; set; }
    }
}
