using Core.Entities;
using Core.Interfaces;
using Ifrastructure.Data;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Api.Tests.StoreContextTests
{
    public class StoreContextTests
    {
        StoreContext storeContext;
        public StoreContextTests()
        {
            storeContext = new StoreContext(new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase("test").Options);

            this.SeedTestProducts();
        }


        [Fact]
        public  void AddEmptyProduct()
        {

        }

        private void SeedTestProducts()
        {
            Product p1 = new Product();
            p1.Id = 5;
            p1.Name = "TestProduct1";
            p1.Description = "TestProduct1";
            p1.Price = 15.55m;
            p1.PictureUrl = "/images/products/1.jpg";
            p1.ProductType = new ProductType() { Name = "Shoes", Id = 1 };
            p1.ProductTypeId = 1;
            p1.ProductGenderBase = new ProductGenderBase() { Name = "Men", Id = 1 };
            p1.ProductGenderBaseId = 1;

            this.storeContext.Add(p1);
            this.storeContext.SaveChangesAsync();
        }
    }
}
