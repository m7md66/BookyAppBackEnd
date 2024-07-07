using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
  
        public class FavoriteUserBooks : EntityBase
        {
            public Guid BookId { get; set; }
            public string UserId { get; set; }

            [ForeignKey(nameof(BookId))]
            public virtual Book Book { get; set; }

            [ForeignKey(nameof(UserId))]
            public virtual ApplicationUser User { get; set; }
        }

    
}
