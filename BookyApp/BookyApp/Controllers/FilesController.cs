using Application;
using Application.Contracts.Services;
using Application.DTOs;
using Application.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BookyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : BaseController
    {
        private readonly IEmailService _emailService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public FilesController(IEmailService emailService, IStringLocalizer<SharedResource> localizer) {
        _emailService = emailService;
        _localizer = localizer;
        }


        [HttpPost("upload")]
        public IActionResult Upload(IFormFile file)
        {
            //var ss = _userId;
            if (file == null || file.Length == 0)
            {
                return BadRequest(_localizer[MessageKeys.NoFileSelected].Value);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Books", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/Books/{fileName}";

            return Ok(fileUrl);
        }
        [HttpPost("SendMail")]
        public Task<ApiResponse<bool>> SendMail()
        {
           

            return _emailService.sendMailAsync();
        }

    }
}
