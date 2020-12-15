using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Errors;
using Api.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
         ITokenService tokenService,
        IMapper mapper,
        RoleManager<IdentityRole> roleManager)
        {
            this._mapper = mapper;
            this.roleManager = roleManager;
            this._tokenService = tokenService;
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "Default";

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName,
                Role = userRole
                
            };
        }
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindByUserByClaimPrincipleWithAddressAsync(HttpContext.User);
            return _mapper.Map<Address,AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress (AddressDto address)
        {
             var user = await _userManager.FindByUserByClaimPrincipleWithAddressAsync(HttpContext.User);
            user.Address = _mapper.Map<AddressDto,Address>(address);
            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded) return Ok(_mapper.Map<Address,AddressDto>(user.Address));
            return BadRequest("Problem updating the user");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
          
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401,"User with that email doesn't exist!"));
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "Default";
     

            var result = await _signInManager
            .CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401,"Wrong email or password."));
            
            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName,
                Role = userRole
            };
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto regiserDto)
        {
            if(CheckEmailExistsAsync(regiserDto.Email).Result.Value) 
            {
               return new BadRequestObjectResult(new ApiValidationErrorResponse
               {Errors = new []{"Email address is in use!"}});
            }
            var user = new AppUser
            {
                DisplayName = regiserDto.DisplayName,
                UserName = regiserDto.Email,
                Email = regiserDto.Email
            };
            var result = await _userManager.CreateAsync(user, regiserDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "Default";

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email,
                Role = userRole
            };
        }

        [Authorize]
        [HttpGet("checkforadminrole")]
        public async Task<bool> CheckUserForAdminRole()
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
            
           return  await _userManager.IsInRoleAsync(user,"Admin");
        }

        

    }
}