using Application.Contracts.Services;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : BaseController
    {
        private readonly IEmailService _emailService;
        public FilesController(IEmailService emailService) {
        _emailService = emailService;
        }


        [HttpPost("upload")]
        public IActionResult Upload(IFormFile file)
        {
            //var ss = _userId;
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file is selected.");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Books", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var fileUrl = $"https://localhost:7265/{fileName}";

            return Ok(fileUrl);
        }
        [HttpPost("SendMail")]
        public Task<ApiResponse<bool>> SendMail()
        {
           

            return _emailService.sendMailAsync();
        }

    }
}
