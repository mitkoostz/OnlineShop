using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Api.Tests
{
    public class BasketControllerTests
    {
        const string baseUrl = "https://localhost:5001";
        [Fact]
        public async void CreateDeleteBasket()
        {
            var webApplicationFactory = new WebApplicationFactory<Startup>();
            HttpClient client = webApplicationFactory.CreateClient();

            string basketData =
                       File.ReadAllText("../../../Resources/BasketCreateOrUpdateJson.json");

            var content = new StringContent(basketData, Encoding.UTF8, "application/json");

            //Create basket
            var response = await client.PostAsync(baseUrl + "/api/basket", content);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            //Delete basket
            response = await client.DeleteAsync(baseUrl + "/api/basket?id=basket1");
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        }
    }
}
