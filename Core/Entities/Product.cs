using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
       
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public ProductGenderBase ProductGenderBase { get; set; }
        public int ProductGenderBaseId { get; set; }
    }
}
