using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BookGenres:EntityBase
    {
        public Guid BookId { get; set; }
        public Guid GenrId { get; set; }

        [ForeignKey(nameof(BookId))]
        public virtual Book Book { get; set; }

        [ForeignKey(nameof(GenrId))]
        public virtual Genres Genr { get; set; }
    }
}
