using Application.Contracts.Services;
using Application.DTOs;
using Domain.Entities;
using Infra.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookyApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : BaseController
    {
        private readonly IUserInterestsService _userInterestsService;

        public GenresController(IUserInterestsService userInterestsService)
        {
            _userInterestsService = userInterestsService;
        }


        [HttpGet]
        public Task<ApiResponse<Genres>> GetAllInterests()
        {
            return _userInterestsService.GetAllInterests();
        }


        [HttpPost("addInterest")]
        public ApiResponse<Genres> addInterest(string name)
        {
            return _userInterestsService.addInterest(name);
        }


        [HttpPost("GetUserInterests")]
        public ApiResponse<Genres> GetUserInterests()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type.Contains("nameidentifier")).Value;

            return _userInterestsService.GetUserInterests(userId);
        }
    }
}
