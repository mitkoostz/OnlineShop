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
                
                // var data = context.Products.Include(p => p.ProductSizeAndQuantity)
                // .ThenInclude(s => s.Size)
                // .FirstOrDefault();
                // var logger = loggerFactory.CreateLogger("ProductQuantityAndSize");
                // logger.LogCritical(data.ProductSizeAndQuantity.FirstOrDefault().Size.SizeShortName.ToString());

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
                //Seed all product Quantity Sizes with random quantity number
                if (!context.ProductSizeAndQuantity.Any())
                {
                    foreach (var product in context.Products.ToArray())
                    {

                        var productSizesForTheType = context.Sizes.Where(s => s.ProductTypeId == product.ProductTypeId).ToArray();

                        for (int i = 0; i < productSizesForTheType.Length; i++)
                        {
                            var randomQuantity = new Random();

                            var productQuantitySizeData = new ProductSizeAndQuantity()
                            {
                                ProductId = product.Id,
                                SizeId = productSizesForTheType[i].Id,
                                Quantity = randomQuantity.Next(1, 100)
                            };

                            context.ProductSizeAndQuantity.Add(productQuantitySizeData);

                        }

                        context.SaveChanges();
                    }

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
