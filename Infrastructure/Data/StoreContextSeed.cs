using Core.Entities;
using Core.Entities.ProductSizeAndQuantityNameSpace;
using Ifrastructure.Data;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            
            try
            {

                //  context.Products.Include(p => p.ProductSizeAndQuantity).FirstOrDefault().ProductSizeAndQuantity.Add(new ProductSizeAndQuantity()
                //  {
                //     ProductId = 1,
                //     SizeId = 1,
                //     Quantity = 5
                //  });
                //  context.SaveChanges();
                var data = context.Products.Include(p => p.ProductSizeAndQuantity)
                .ThenInclude(s => s.Size)
                .FirstOrDefault();
                var logger = loggerFactory.CreateLogger("ProductQuantityAndSize");
                logger.LogCritical(data.ProductSizeAndQuantity.FirstOrDefault().Size.SizeShortName.ToString());
                
                if (!context.Sizes.Any())
                {
                    var sizeData = 
                        File.ReadAllText("../Infrastructure/Data/SeedData/ProductSizes.json");
                    var sizes = JsonSerializer.Deserialize<List<Size>>(sizeData);
                    foreach (var size in sizes)
                    {
                        context.Sizes.Add(size);
                    }
                    await context.SaveChangesAsync();
                }
         
                if (!context.ProductGenderBase.Any())
                {
                    var brandsData = 
                        File.ReadAllText("../Infrastructure/Data/SeedData/GenderBase.json");
                    var brands = JsonSerializer.Deserialize<List<ProductGenderBase>>(brandsData);
                    foreach (var item in brands)
                    {
                        context.ProductGenderBase.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.ProductTypes.Any())
                {
                    var typesData =
                        File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    foreach (var item in types)
                    {
                        context.ProductTypes.Add(item);
                    }
                    await context.SaveChangesAsync();

                }

                    if (!context.DeliveryMethods.Any())
                {
                    var dmData =
                        File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");
                    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                    foreach (var item in methods)
                    {
                        context.DeliveryMethods.Add(item);
                    }
                    await context.SaveChangesAsync();

                }

                if (!context.Products.Any())
                {
                    var productsData =
                        File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    foreach (var item in products)
                    {
                        context.Products.Add(item);
                    }
                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
                
            }
        }
    }
}
