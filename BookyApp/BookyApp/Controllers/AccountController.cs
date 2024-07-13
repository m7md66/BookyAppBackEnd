using Application.Contracts.Services;
using Application.DTOs.UserDto;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


namespace BookyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IAccountService accountService, UserManager<ApplicationUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;

        }


        //[HttpGet("GetRoles")]
        //public async Task<IActionResult> GetRoles()
        //{
        //    return Ok(await _accountService.GetAllRoles());
        //}
        [Authorize]
       
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromForm] AddUserRequest request)
        {
            var ss = User.Claims.FirstOrDefault(a=>a.Type.Contains("nameidentifier")).Value;
            var ssd = User.Claims;
            var response = await _accountService.AddUser(request);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequest dto)
        {
            var response = await _accountService.Login(dto);

            return Ok(response);
        }



    }


}
