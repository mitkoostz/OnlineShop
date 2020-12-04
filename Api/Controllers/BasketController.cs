using System;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Errors;
using Api.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            this._mapper = mapper;
            this._basketRepository = basketRepository;
        }

        [HttpGet]
        [LimitRequests(20, 60, 180)]
        public async Task<ActionResult<CustomerBasket>> GetBasketId(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        [LimitRequests(5, 40, 30,useTestMode:true)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
         {
            var customerBasket = _mapper.Map<CustomerBasketDto,CustomerBasket>(basket);
            var updatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);
            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
    }
}