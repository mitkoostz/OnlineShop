using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Api.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Ifrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<ProductType> productTypesRepo;
        private readonly IGenericRepository<ProductGenderBase> productGenderRepo;


        public AdminController(
            IWebHostEnvironment env,
            IUnitOfWork unitOfWork,
            IGenericRepository<ProductType> productTypesRepo,
            IGenericRepository<ProductGenderBase> productGenderRepo)
        {
            this.env = env;
            this.unitOfWork = unitOfWork;
            this.productTypesRepo = productTypesRepo;
            this.productGenderRepo = productGenderRepo;
        }

        [HttpPut("addnewproduct")]
        public async Task<ActionResult> AddNewProduct([FromForm] AddNewProductDto product)
        {

            string fName = product.productImage.FileName;
            //TODO  if type sent by admin is not in table , create new Type of Cloth
            var type = await productTypesRepo.GetEntityWithSpec(new ProductTypeByProductTypeName(product.ProductType));
            var gender = await productGenderRepo.GetEntityWithSpec(new GetGenderByName(product.ProductGenderBase));

            string rootPath = "images/ProductImages/";
            switch (type.Name)
            {
                case "T-shirts":
                    rootPath += "T-shirts/";
                        break;
                case "Jeans":
                    rootPath += "Jeans/";
                    break;
                case "Shoes":
                    rootPath += "Shoes/";
                    break;
                case "Hats":
                    rootPath += "Hats/";
                    break;
                default:
                    rootPath += "Default/";
                    break;
            }

            string path = Path.Combine(env.WebRootPath, rootPath + fName);
            string ProductImageRootPath = rootPath + fName;

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await product.productImage.CopyToAsync(stream);
            }
            Product productToBeAdded = new Product()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ProductGenderBase = gender,   //context.ProductGenderBase.Find(product.ProductGenderBaseId),
                ProductType = type,
                PictureUrl = ProductImageRootPath,

            };
            
              unitOfWork.Repository<Product>().Add(productToBeAdded);
              await unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete("deleteproduct")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
           var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);
           unitOfWork.Repository<Product>().Delete(product);
           await unitOfWork.Complete();

            return Ok();
        }

        [HttpPut("updateproduct")]
        public async Task<ActionResult> UpdateProduct([FromForm]ProductToUpdateDto productUpdateData)
        {
            var product = await unitOfWork.Repository<Product>().GetByIdAsync(productUpdateData.Id);
            if (product == null) { return Ok(); }

            product.Name = productUpdateData.Name ?? product.Name;
            product.Description = productUpdateData.Description ?? product.Description;
            product.Price = productUpdateData.Price ?? product.Price;
            product.ProductGenderBaseId = productUpdateData.GenderBaseId ?? product.ProductGenderBaseId;
            ProductType type = await productTypesRepo.GetEntityWithSpec(new ProductTypeByProductTypeName(productUpdateData.ProductType));
            if(type == null) { } else { product.ProductTypeId = type.Id;  }


            string path = Path.Combine(env.WebRootPath, product.PictureUrl);
            if(productUpdateData.ProductImage != null)
            {
               using (var stream = new FileStream(path, FileMode.Create))
               {
                await productUpdateData.ProductImage.CopyToAsync(stream);
               }
            }
            unitOfWork.Repository<Product>().Update(product);
            await unitOfWork.Complete();
            return Ok();
        }


    }
}
