using Application.Contracts.Services;
using Application.DTOs;
using Application.DTOs.interests;
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
        //git rm --cached*.dll
        private readonly IUserInterestsService _userInterestsService;

        public GenresController(IUserInterestsService userInterestsService)
        {
            _userInterestsService = userInterestsService;
        }


        [HttpGet("GetAllInterests")]
        public Task<ApiResponse<List<InterestsResponse>>> GetAllInterests()
        {
            return _userInterestsService.GetAllInterests();
        }


        [HttpPost("addInterest")]
        public Task<ApiResponse<Genres>> addInterest(string name)
        {
            return _userInterestsService.addInterest(name);
        }


        [HttpGet("GetUserInterests")]
        public ApiResponse<Genres> GetUserInterests()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type.Contains("nameidentifier")).Value;
            return _userInterestsService.GetUserInterests(userId);
        }



        [HttpPost("MakeInterest")]
        public Task<ApiResponse<bool>> MakeInterest(Guid GenreId) { 
           return  _userInterestsService.MakeInterest(GenreId);
        }


        [HttpPost("MakeInterests")]
        public Task<ApiResponse<bool>> MakeInterests(List<Guid> GenreIds)
        {
            return _userInterestsService.MakeInterests(GenreIds);

        }



    }
}
