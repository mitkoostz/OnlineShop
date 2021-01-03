using Api.Dtos;
using AutoMapper;
using AutoMapper.Configuration;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Api.Dtos.ProductDetails;

namespace Api.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto,string>
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        public ProductUrlResolver(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            this._config = config;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }
            return null;
        }
    }
      public class ProductDetailUrlResolver : IValueResolver<Product, ProductDetailToReturnDto,string>
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        public ProductDetailUrlResolver(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            this._config = config;
        }
        public string Resolve(Product source, ProductDetailToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }
            return null;
        }
    }
}
