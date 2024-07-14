using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Interest:EntityBase
    {
        public string Name { get; set; }
        public virtual ICollection<UserInterests> UserInterests { get; set; }
    }
}
