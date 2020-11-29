using Core.Entities;
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


namespace Api.Tests.PaymentServiceTests
{
    public class PaymentServiceTests
    {
        public PaymentServiceTests()
        {
            this.config = Config.InitConfiguration();
            this.ApiSecretKey = config["ApiSecretKey"];
        }
        private readonly IConfiguration config;
        private readonly string ApiSecretKey;

        [Fact]
        public async void TestPaymentWithEmptyBasket()
        {
            //Test creating payment with empty basket returns null
            var basketRepo = new Mock<IBasketRepository>();
            basketRepo.Setup(b => b.GetBasketAsync("1")).Returns(GetNullBasketAsync());

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(u => u.Repository<DeliveryMethod>().GetByIdAsync(1)).Returns(GetDeliveryMethodAsync());
            unitOfWork.Setup(u => u.Repository<Product>().GetByIdAsync(1)).Returns(GetProductAsync());

            var config = new Mock<IConfiguration>();
            config.Setup(c => c[It.IsAny<string>()]).Returns(ApiSecretKey);

            PaymentService paymentService = new PaymentService(
                basketRepo.Object,
                unitOfWork.Object,
                config.Object);

            var result = await paymentService.CreateOrUpdatePaymentIntent("1");

            Assert.Null(result);
            
        }
        [Fact]
        public async void TestBasketWithNullItemAndNoIdItem()
        {
            var basketRepo = new Mock<IBasketRepository>();
            basketRepo.Setup(b => b.GetBasketAsync("1")).Returns(GetBasketWithNullItemAndNoIdItemAsync());

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(u => u.Repository<DeliveryMethod>().GetByIdAsync(1)).Returns(GetDeliveryMethodAsync());
            unitOfWork.Setup(u => u.Repository<Product>().GetByIdAsync(1)).Returns(GetProductAsync());

            var config = new Mock<IConfiguration>();
            config.Setup(c => c[It.IsAny<string>()]).Returns(ApiSecretKey);

            PaymentService paymentService = new PaymentService(
                basketRepo.Object,
                unitOfWork.Object,
                config.Object);

            var result = await paymentService.CreateOrUpdatePaymentIntent("1");

            //We have 2 items - 1 empty and 1 without id we delete both and return null
            Assert.Null(result);

        }
        [Fact]
        public async void TestBasketWithValidItemAndEmptyItem()
        {
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
                config.Object);

            var result = await paymentService.CreateOrUpdatePaymentIntent("1");

            //We have 2 items - 1 valid and 1 empty item with no seted properties, we delete the second
            Assert.Single(result.Items);

        }
        [Fact]
        public async void TestRealProductPriceAndDelvieryMethodCostAndPaymentIntentAndClientSecretCreations()
        {
            //Test check if user input fake item price in basket and/or delvierymethod price he will be charged with real item by id if found
            //also checks for creations of intent id and client secret
            //This test creates uncomplete payment Intent and Cancel It

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
   
            // Order total must be 5x200$ item cost + 15$ ship costs = 1015$ * 100
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
            items.Add(new BasketItem());

            customerBasket.Items = items;


            return await Task.FromResult(customerBasket);
        }

        private async Task<CustomerBasket> GetBasketWithNullItemAndNoIdItemAsync()
        {
            CustomerBasket customerBasket = new CustomerBasket
            {
                DeliveryMethodId = 1,
                ShippingPrice = 10
            };
            var items = new List<BasketItem>();
            items.Add(new BasketItem
            {
                ProductName = "Shoes",
                ProductGenderBase = "Men",
                Price = 155,
                Quantity = 5,
                PictureUrl = "test",
                Type = "Shoes"
            });
            items.Add(new BasketItem());

            customerBasket.Items = items;


            return await Task.FromResult(customerBasket);
        }
        private async Task<CustomerBasket> GetNullBasketAsync()
        {
            CustomerBasket customerBasket = null;
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
