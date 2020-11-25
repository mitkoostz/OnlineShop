﻿using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Product = Core.Entities.Product;


namespace Api.Tests
{
    public class PaymentServiceTests
    {
        const string ApiSecretKey = "sk_test_51HblC0FUcQA4Qn1XN2XkdOLNjt67jqvLhvOXekNVPlwtFyvS1QbscAAhwYx5O2DjqipDVIetjoQdq7ydP3Ke3Ydw006awifri3";

        [Fact]
        public async void TestRealProductPriceAndDelvieryMethodCostAndPaymentIntentAndClientSecretCreations()
        {
            //Test check if user input fake item price in basket and/or delvierymethod price he will be charged with real item by id if found
            //also checks for creations of intent id and client secret
            //This test creates uncomplete payment Intent and Cancel It
            var customerBasket = new CustomerBasket();

            var basketRepo = new Mock<IBasketRepository>();
            basketRepo.Setup(b => b.GetBasketAsync("1")).Returns(GetBasketAsync());

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(u => u.Repository<DeliveryMethod>().GetByIdAsync(1)).Returns(GetDeliveryMethodAsync());
            unitOfWork.Setup(u => u.Repository<Product>().GetByIdAsync(1)).Returns(GetProductAsync());

            var config = new Mock<IConfiguration>();
            config.Setup(c => c[It.IsAny<string>()]).Returns(ApiSecretKey);

            PaymentService paymentService = new PaymentService(
                basketRepo.Object,
                unitOfWork.Object,
                config.Object );

            var result = await paymentService.CreateOrUpdatePaymentIntent("1");

            //When payment is made we find deliveryMethod by Id with real price
            //Basket shipping Price is 10$ but actual deliveryMethod costs 15$
            Assert.Equal(15, result.ShippingPrice); 

            //When payment is made every product in basket is comapered with product in db
            //Product Price in basket is set to 155$ but the product by id in Database costs 200$
            Assert.Equal(200, result.Items.FirstOrDefault().Price);


            //paymentIntentId Created
            Assert.False(string.IsNullOrEmpty(result.PaymentIntentId));

            //client secret created by Stripe
            Assert.False(string.IsNullOrEmpty(result.ClientSecret));

            PaymentIntent intent = GetPaymentIntent(result.PaymentIntentId);
            var amountPaid = intent.Amount;
   
            Assert.Equal(amountPaid, 1015*100);


            CancelPaymentIntent(result.PaymentIntentId);
        }

        private PaymentIntent GetPaymentIntent(string paymentIntentId)
        {
            StripeConfiguration.ApiKey = ApiSecretKey;
            var service = new PaymentIntentService();
            PaymentIntent intent = service.Get(paymentIntentId);
          
             return intent;
        }
        private void CancelPaymentIntent(string paymentIntentId)
        {
            StripeConfiguration.ApiKey = ApiSecretKey;

            var service = new PaymentIntentService();
            service.Cancel(paymentIntentId);
        }

        private async Task<Product> GetProductAsync()
        {
            var product = new Product() { Price = 200 };
            return await Task.FromResult(product);
           
        }

        private async Task<CustomerBasket> GetBasketAsync()
        {
            CustomerBasket customerBasket = new CustomerBasket
            {
                DeliveryMethodId = 1,
                ShippingPrice = 10
            };
            var items = new List<BasketItem>();
            items.Add(new BasketItem
            {
                Id = 1,
                ProductName = "Shoes",
                ProductGenderBase = "Men",
                Price = 155,
                Quantity = 5,
                PictureUrl = "test",
                Type = "Shoes"
            });
            customerBasket.Items = items;


            return await Task.FromResult(customerBasket);
        }

        private async Task<DeliveryMethod> GetDeliveryMethodAsync()
        {
           return await Task.FromResult(new DeliveryMethod(){
               Id = 1,
               ShortName = "test",
               Price = 15,
               DeliveryTime = "1-2wek",
               Description = "notfast"
           });

        }

    }
}