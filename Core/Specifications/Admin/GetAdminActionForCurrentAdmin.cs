using Core.Entities.Admin;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Specifications.Admin
{
    public class GetAdminActionForCurrentAdmin : BaseSpecification<AdminActionHistory>
    {
        public GetAdminActionForCurrentAdmin(string email) 
            : base(ac => ac.AdminEmail == email)
        {
            AddOrderByDescending(ac => ac.Date);
            ApplyPaging(0, 15);
            
        }
    }
}
