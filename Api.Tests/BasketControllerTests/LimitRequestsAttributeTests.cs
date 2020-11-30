using Core.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using Xunit;

namespace Api.Tests.BasketControllerTests
{
    public class LimitRequestsAttributeTests
    {
        public LimitRequestsAttributeTests()
        {
            this.config = Config.InitConfiguration();
            this.apiUrl = config["ApiUrl"];
            this.webApplicationFactory = new WebApplicationFactory<Startup>();
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
        public async void Create40BasketsBadRequestAfter20thRequest()
        {
            HttpClient client = webApplicationFactory.CreateClient();
           
            for (int i = 1; i <= 40; i++)
            {
                string basket = JsonSerializer.Serialize(CreateTestBasket(5, 2, 1, 15, i.ToString()));
                var content = new StringContent(basket, Encoding.UTF8, "application/json");      
                //Create basket test
                var response = await client.PostAsync(basketControllerRoot, content);
                if(i > 20)
                {
                    Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
                }
            }
            for (int i = 1; i <= 40; i++)
            {

                await client.DeleteAsync(basketControllerRoot + $"?id={i}");
            }
        }

        [Fact]
        public async void Create19RequestsWaitAndCreate19More()
        {
            HttpClient client = webApplicationFactory.CreateClient();

            for (int i = 1; i <= 38; i++)
            {
                string basket = JsonSerializer.Serialize(CreateTestBasket(5, 2, 1, 15, i.ToString()));
                var content = new StringContent(basket, Encoding.UTF8, "application/json");
                //Create basket test
                var response = await client.PostAsync(basketControllerRoot, content);
                if (i == 19)
                {
                    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
                    Thread.Sleep(10000);
                }
                Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            }
            for (int i = 1; i <= 40; i++)
            {

                await client.DeleteAsync(basketControllerRoot + $"?id={i}");
            }
        }

        [Fact]
        public async void Create25RequestAndAfterBanTryAgain()
        {
            HttpClient client = webApplicationFactory.CreateClient();

            for (int i = 1; i <= 39; i++)
            {
                string basket = JsonSerializer.Serialize(CreateTestBasket(5, 2, 1, 15, i.ToString()));
                var content = new StringContent(basket, Encoding.UTF8, "application/json");
                //Create basket test
                var response = await client.PostAsync(basketControllerRoot, content);
                if (i == 20)
                {
                    Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
                    Thread.Sleep(30000);
                }
                if(i > 20)
                {
                    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
                }
            }
            for (int i = 1; i <= 40; i++)
            {

                await client.DeleteAsync(basketControllerRoot + $"?id={i}");
            }
        }


        private CustomerBasket CreateTestBasket(
                        int numberItemsInBasket,
                        int quantityOfEveryItem,
                        int idOfEveryItem,
                        decimal priceOfEveryItem,
                        string basketId)
        {
            CustomerBasket basket = new CustomerBasket() { Id = basketId };

            for (int i = 0; i < numberItemsInBasket; i++)
            {
                basket.Items.Add(new BasketItem()
                {
                    ProductName = "Test",
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
