using System.Threading.Tasks;
using Api.Dtos.ContactUs;
using Api.Errors;
using AutoMapper;
using Core.Entities.ContactUs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ContactUsController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ContactUsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpPost("contactusmessage")]
        public async Task<ActionResult> SendUsMessage(ContactUsMessageDto message)
        {
            var enitityMessage = mapper.Map<ContactUsMessageDto,ContactUsMessage>(message);
            unitOfWork.Repository<ContactUsMessage>().Add(enitityMessage);
            await unitOfWork.Complete();
            return Ok(new ApiResponse(200,"Thank you for contacting us! We will reply as soon as posibble!"));

        }
    }
}