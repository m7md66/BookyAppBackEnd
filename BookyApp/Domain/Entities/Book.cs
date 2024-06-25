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
    public class Book: EntityBase
    {
       
       public string UserId { get; set; }
        public string Title { get; set; }
        public string Auther { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
        //public virtual ICollection<FavoriteBook> FavoriteBooks { get; set; }
        public virtual ICollection<Quotation> Quotations { get; set; }


    }
}
