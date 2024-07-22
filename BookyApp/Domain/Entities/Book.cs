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
       
      
        public string Title { get; set; }
        public string Auther { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public DateTime PublicationDate { get; set; }


        public virtual ICollection<FavoriteUserBooks> FavoriteBooks { get; set; }
        public virtual ICollection<Quotation> Quotations { get; set; }
        public virtual ICollection<BookGenres> BookGenres { get; set; }


    }
}
