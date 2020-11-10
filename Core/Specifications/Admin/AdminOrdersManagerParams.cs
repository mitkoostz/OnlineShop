using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Admin
{
    public  class AdminOrdersManagerParams
    {
        public string NameSearch { get; set; }
        public DateTime? DateSearch { get; set; }
        public string EmailSearch { get; set; }
        public string PaymentIntentSearch { get; set; }

    }
}
