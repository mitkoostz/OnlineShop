using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Dtos
{
    public class AddNewProductDto
    {   
        [FromForm]
        public string Name { get; set; }
        [FromForm]
        public string Description { get; set; }
        [FromForm]
        public decimal Price { get; set; }
        [FromForm]
        public IFormFile productImage { get; set; }
        public string ProductType { get; set; }
        [FromForm]
        public int ProductTypeId { get; set; }
        [FromForm]
        public string ProductGenderBase { get; set; }
    }
}
