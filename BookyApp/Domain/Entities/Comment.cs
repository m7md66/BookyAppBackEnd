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
    public class Comment:EntityBase
    {
       public string UserId { get; set; }
        public Guid QuotationId { get; set; }
        public string Content { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser user { get; set; }

        [ForeignKey(nameof(QuotationId))]
        public Quotation quotation { get; set; }



    }
}
