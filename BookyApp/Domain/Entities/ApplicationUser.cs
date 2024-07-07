


using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
  

        public class ApplicationUser :IdentityUser
    {
            public ApplicationUser()
            {
               
            }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName => $"{FirstName} {LastName}";
            public string ImageUrl { get; set; }
            public string ImageName { get; set; }
            public string ImageExtention { get; set; }
            public DateTime CreatedDate { get; set; } = DateTime.Now;
            public string CreatedBy { get; set; }
           
       
        public virtual ICollection<Quotation> Quotations { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ReQuote> ReQuotes { get; set; }
        public virtual ICollection<QuotationLike> QuotationLikes { get; set; }
        public virtual ICollection<QuotationShare> QuotationShares { get; set; }
        public virtual ICollection<FavoriteUserBooks> favoriteUserBooks { get; set; }
        public virtual ICollection<UserInterests> UserInterests { get; set; }


    }



    
}
