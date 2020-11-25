﻿using Core.Entities.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
       
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(600)]
        public string Description { get; set; }
        [Required]
        [MaxLength(5000)]
        public decimal Price { get; set; }
        [Required]
        public string PictureUrl { get; set; }

        public ProductType ProductType { get; set; }
        [Required]
        public int ProductTypeId { get; set; }

        public ProductGenderBase ProductGenderBase { get; set; }
        [Required]
        public int ProductGenderBaseId { get; set; }
        public List<AdminActionHistory> ProductAdminHistory { get; set; }
    }
}
