using System.Collections.Generic;
using Core.Entities.Reviews;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        
        public Address Address { get; set; }

        public List<ProductReview> Reviews { get; set; }
    }
}