using Core.Entities.Identity;
using System;

namespace Core.Entities.Admin
{
    public class AdminActionHistory : BaseEntity
    {
        public DateTime Date { get; set; }
        public string AdminEmail { get; set; }
        public AdminActionOperations Operation { get; set; }
        public string AdminAction { get; set; }
        public string UserId { get; set; }
        public  Product Product { get; set; }
        public int? ProductId { get; set; }

    }
}