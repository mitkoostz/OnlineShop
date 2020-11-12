using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Dtos.AdminPanel;
using Api.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Entities.Admin;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Core.Specifications.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<ProductType> productTypesRepo;
        private readonly IGenericRepository<ProductGenderBase> productGenderRepo;
        private readonly IGenericRepository<AdminActionHistory> adminActionHistoryRepo;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public AdminController(
            IWebHostEnvironment env,
            IUnitOfWork unitOfWork,
            IGenericRepository<ProductType> productTypesRepo,
            IGenericRepository<ProductGenderBase> productGenderRepo,
            IGenericRepository<AdminActionHistory> adminActionHistoryRepo,
            UserManager<AppUser> userManager,
            IMapper mapper
            )
        {
            this.env = env;
            this.unitOfWork = unitOfWork;
            this.productTypesRepo = productTypesRepo;
            this.productGenderRepo = productGenderRepo;
            this.adminActionHistoryRepo = adminActionHistoryRepo;
            this.userManager = userManager;
            this.mapper = mapper;
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

            var user = await userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

            AdminActionHistory reg = new AdminActionHistory();
            reg.AdminEmail = user.Email;
            reg.Date = DateTime.Now;
            reg.Operation = AdminActionOperations.Create;
            reg.AdminAction = "Added product: " + product.Name;
            reg.UserId = user.Id;
            adminActionHistoryRepo.Add(reg);


            await unitOfWork.Complete();
            return Ok();
        }

        [HttpDelete("deleteproduct")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
           var product = await unitOfWork.Repository<Product>().GetEntityWithSpec(new GetAllAdminActionForProduct(id));

           unitOfWork.Repository<Product>().Delete(product);

            var user = await userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

            AdminActionHistory reg = new AdminActionHistory();
            reg.AdminEmail = user.Email;
            reg.Date = DateTime.Now;
            reg.Operation = AdminActionOperations.Delete;
            reg.AdminAction = "Deleted product: " + product.Name;
            reg.UserId = user.Id;
             adminActionHistoryRepo.Add(reg);

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
            product.ProductGenderBase = await productGenderRepo.GetEntityWithSpec(new GetGenderByName(productUpdateData.ProductGenderBase));
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
            var user = await userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

            AdminActionHistory reg = new AdminActionHistory();
            reg.AdminEmail = user.Email;
            reg.Date = DateTime.Now;
            reg.Operation = AdminActionOperations.Update;
            reg.AdminAction = "Updated product: " + product.Name;
            reg.UserId = user.Id;
            reg.Product = product;
            adminActionHistoryRepo.Add(reg);

            unitOfWork.Repository<Product>().Update(product);
            await unitOfWork.Complete();
            return Ok();
        }


        [HttpGet("adminhistory")]
        public async Task<ActionResult<IReadOnlyList<AdminActionHistory>>> GetAdminActionHistory()
        {
            var user = await userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

            return Ok(await unitOfWork.Repository<AdminActionHistory>()
                .ListAsync(new GetAdminActionForCurrentAdmin(user.Email)));
        }

        [HttpGet("getorders")]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrders([FromQuery] AdminOrdersManagerParams ordersParams)
        {
            var orders = await unitOfWork.Repository<Order>().ListAsync(new GetOrdersWithSearchAndIncludes(ordersParams));
            if (ordersParams.DateSearch.HasValue)
            {
                orders = orders.Where(o => 
                 o.OrderDate.Year == ordersParams.DateSearch.Value.Year &&
                 o.OrderDate.Month == ordersParams.DateSearch.Value.Month &&
                 o.OrderDate.Day == ordersParams.DateSearch.Value.Day).ToList();
            }
    

            return Ok(orders);
        }

        [HttpGet("getordersfordayweekmounth")]
        public async Task<ActionResult<OrdersForDayWeekMounth>> GetOrdersForDayWeekMounth()
        {
            var orders = await unitOfWork.Repository<Order>().ListAsync(new GetLast30DayOrders());

            OrdersForDayWeekMounth result = new OrdersForDayWeekMounth();
            result.OrdersMounth = orders.Count;
            result.OrdersWeek = orders.Where(o => o.OrderDate >= DateTime.Now.AddDays(-7)).Count();
            result.OrdersToday = orders.Where(o => o.OrderDate.DayOfYear >= DateTime.Now.DayOfYear).Count();

            return Ok(result);
        }

        [HttpGet("getproducts")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams par)
        {
            var products = await unitOfWork.Repository<Product>().ListAsync(new GetProductsSpecification(par));
            var data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(data);
        }

    }
}
