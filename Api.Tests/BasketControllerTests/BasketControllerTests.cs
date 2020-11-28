using Core.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Api.Tests.BasketControllerTests
{
    public class BasketControllerTests
    {
        public BasketControllerTests()
        {
            this.config = Config.InitConfiguration();
            this.apiUrl = config["ApiUrl"];
            this. webApplicationFactory = new WebApplicationFactory<Startup>();
            this.basketControllerRoot = apiUrl + "api/basket";

        }

        private readonly IConfiguration config;
        private readonly string apiUrl;
        private readonly string basketControllerRoot;
        private readonly WebApplicationFactory<Startup> webApplicationFactory;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [Fact]
        public async void CreateGetDeleteBasketTest()
        {
            HttpClient client = webApplicationFactory.CreateClient();
            string basket = JsonSerializer.Serialize(CreateTestBasket(5, 2, 1, 15, "basketTest"));
            var content = new StringContent(basket, Encoding.UTF8, "application/json");
            //Create basket test
            var response = await client.PostAsync(basketControllerRoot,content);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            //Get basket test - 5 items / every item has 2 quantity and 15$ price
            response = await client.GetAsync(basketControllerRoot + "?id=basketTest");
            var responseJson = await response.Content.ReadAsStringAsync();
            var createdBasket = JsonSerializer.Deserialize<CustomerBasket>(responseJson,options);
            Assert.Equal(5, createdBasket.Items.Count);
            createdBasket.Items.ForEach(item => 
            {
               Assert.Equal(2, item.Quantity);
               Assert.Equal(15, item.Price); 
            });
            //Delete basket test
            response = await client.DeleteAsync(basketControllerRoot + "?id=basketTest");
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async void Max15ItemsInBasketTest()
        {
             HttpClient client = webApplicationFactory.CreateClient();
             string basket = JsonSerializer.Serialize(CreateTestBasket(100,5,1,15,"basketTest"));
             var content = new StringContent(basket, Encoding.UTF8, "application/json");
             var response = await client.PostAsync(basketControllerRoot, content);
            // The max is 15 items in Basket
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);          
        }
        [Fact]
        public async void Max20QuantityPerItemTest()
        {
            HttpClient client = webApplicationFactory.CreateClient();
            string basket = JsonSerializer.Serialize(CreateTestBasket(2, 25, 1, 15, "basketTest"));
            var content = new StringContent(basket, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(basketControllerRoot, content);
            // The max is 20 quantity for every item in basket
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async void Max15ItemsAnd20QuantityPerItemTest()
        {
            HttpClient client = webApplicationFactory.CreateClient();
            string basket = JsonSerializer.Serialize(CreateTestBasket(100, 25, 1, 15, "basketTest"));
            var content = new StringContent(basket, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(basketControllerRoot, content);
            // The max is 15 items in Basket and 20 quantity for every item in basket
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async void BasketWithSameIdIsUpdatedTest()
        {
            HttpClient client = webApplicationFactory.CreateClient();
            string basket = JsonSerializer.Serialize(CreateTestBasket(2, 3, 1, 15, "basketTest"));
            var content = new StringContent(basket, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(basketControllerRoot, content);
            var responseJson = await response.Content.ReadAsStringAsync();
            var createdBasket = JsonSerializer.Deserialize<CustomerBasket>(responseJson,options);
            //Create Basket test - 2 items in basket
            Assert.Equal("basketTest", createdBasket.Id);
            Assert.Equal(2, createdBasket.Items.Count);
            string secondBasket = JsonSerializer.Serialize(CreateTestBasket(5, 3, 1, 15, "basketTest"));
            var secondContent = new StringContent(secondBasket, Encoding.UTF8, "application/json");
            response = await client.PostAsync(basketControllerRoot, secondContent);
            responseJson = await response.Content.ReadAsStringAsync();
            var updatedBasket = JsonSerializer.Deserialize<CustomerBasket>(responseJson,options);
            //Update basket test - 5 items in basket
            Assert.Equal("basketTest", updatedBasket.Id);
            Assert.Equal(5, updatedBasket.Items.Count);
            response = await client.DeleteAsync(basketControllerRoot + "?id=basketTest");
            //delete the basket
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        private CustomerBasket CreateTestBasket(
            int numberItemsInBasket,
            int quantityOfEveryItem,
            int idOfEveryItem,
            decimal priceOfEveryItem,
            string basketId)
        {
            CustomerBasket basket = new CustomerBasket(){Id = "basketTest"};

            for (int i = 0; i < numberItemsInBasket; i++)
            {
                basket.Items.Add(new BasketItem(){
                    ProductName = basketId,
                    Price = priceOfEveryItem,
                    Id = idOfEveryItem,
                    ProductGenderBase = "Men",
                    PictureUrl = "test.png",
                    Type = "Shoes",
                    Quantity = quantityOfEveryItem
                });
            }

            return basket;
        }
    }
}
