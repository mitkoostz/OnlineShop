using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos
{
    public class ProductToUpdateDto
    {
        [FromForm]
        public int Id { get; set; }
        
        [FromForm]
        public string Name { get; set; }
        [FromForm]
        public string Description { get; set; }
        [FromForm]
        public decimal? Price { get; set; }
        [FromForm]
        public IFormFile ProductImage { get; set; }
        public string ProductType { get; set; }
        [FromForm]
        public int? ProductTypeId { get; set; }
        [FromForm]
        public string ProductGenderBase { get; set; }

    }
}
