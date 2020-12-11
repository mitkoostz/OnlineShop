using Api.Dtos;
using Api.Dtos.ContactUs;
using Api.Dtos.ProductReviews;
using AutoMapper;
using Core.Entities;
using Core.Entities.ContactUs;
using Core.Entities.OrderAggregate;
using Core.Entities.Reviews;

namespace Api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductGenderBase, o => o.MapFrom(s => s.ProductGenderBase.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto,CustomerBasket>();
            CreateMap<BasketItemDto,BasketItem>();
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
            CreateMap<ContactUsMessageDto, ContactUsMessage>();
            CreateMap<ProductReview, ProductReviewReturnDto>();

            CreateMap<Order,OrderToReturnDto>()
                  .ForMember( d => d.DeliveryMethod , o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                  .ForMember( d => d.ShippingPrice , o => o.MapFrom(s => s.DeliveryMethod.Price)); 

            CreateMap<OrderItem,OrderItemDto>()
                  .ForMember( d => d.ProductId , o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                  .ForMember( d => d.ProductName , o => o.MapFrom(s => s.ItemOrdered.ProductName))
                  .ForMember( d => d.PictureUrl , o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                  .ForMember( d => d.PictureUrl , o => o.MapFrom<OrderItemUrlResolver>());

        }
    }
}
